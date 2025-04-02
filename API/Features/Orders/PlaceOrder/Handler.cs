using API.HttpClients;
using API.Infrastructure;
using Domain.Database;
using Domain.Database.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Account;
using Domain.ValueObjects.Order;
using Domain.ValueObjects.Payment;
using Domain.ValueObjects.Product;
using Events;
using FluentResults;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneOf;
using Error = Domain.ValueObjects.Error;

namespace API.Features.Orders.PlaceOrder;
public class PlaceOrderHandlerRequest
{
    private PlaceOrderHandlerRequest(){ }

    public AccountId AccountId { get; private set; }
    public IEnumerable<(Sku sku, BigInt amount)> Products{ get; private set; } = null!;
    public VisaCard VisaCard { get; private set; }

    public static Result<PlaceOrderHandlerRequest> Create(
        string? accountId, 
        IEnumerable<(string? sku, int amount)> products,
        (string? owner, string? cardNumber, int expirationMonth, int expirationYear, int cvv) visaCard
        )
    {
        List<Result> results = [];
        var voAccountId = AccountId.Create(accountId);
        results.Add(voAccountId.ToResult());

        var validatedProducts = new List<(Sku, BigInt)>();
        foreach (var product in products)
        {
            Result<Sku> voSku = Sku.Create(product.sku);
            Result<BigInt> voAmount = BigInt.Create(product.amount);

            results.Add(voSku.ToResult());
            results.Add(voAmount.ToResult());
            
            if (voSku.IsSuccess && voAmount.IsSuccess)
            {
                validatedProducts.Add((voSku.Value, voAmount.Value));
            }
        }
        
        // VISA
        var voVisaCard = VisaCard.Create(visaCard.owner, visaCard.cardNumber, visaCard.expirationMonth, visaCard.expirationYear,
            visaCard.cvv);
        results.Add(voVisaCard.ToResult());
        
        var mergedResult = Result.Merge(results.ToResult());
        if (mergedResult.IsFailed)
        {
            return Result.Fail(mergedResult.Errors);
        }

        return Result.Ok(new PlaceOrderHandlerRequest
        {
            AccountId = voAccountId.Value,
            Products = validatedProducts,
            VisaCard = voVisaCard.Value,
        });
    }
}
public interface IPlaceOrderHandler : IHandler
{
    Task<OneOf<(OrderIdentifier identifier, OrderStatus orderStatus), Error>> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken);
}

public class PlaceOrderHandler : IPlaceOrderHandler
{
    private readonly ILogger<PlaceOrderHandler> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IPublishEndpoint _bus;
    private readonly IPriceHttpClient _priceHttpClient;
    private readonly IOrderIdentifierGenerator _orderIdentifierGenerator;

    public PlaceOrderHandler(
        ILogger<PlaceOrderHandler> logger,
        AppDbContext dbContext,
        IPublishEndpoint bus,
        IPriceHttpClient priceHttpClient,
        IOrderIdentifierGenerator orderIdentifierGenerator
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _bus = bus;
        _priceHttpClient = priceHttpClient;
        _orderIdentifierGenerator = orderIdentifierGenerator;
    }

    public async Task<OneOf<(OrderIdentifier identifier, OrderStatus orderStatus), Error>> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        // Outbox pattern now.
        var skus = request.Products.Select(x => x.sku.ToString()).ToArray();

        var productsValidationErrors = new List<Result>();
        var products = await _dbContext.Products
            .Where(p => skus.Contains(p.Sku))
            .ToArrayAsync(cancellationToken);

        if (!products.Any())
        {
            return new Error($"No products were found for SKUs '{string.Join("', '", skus)}'");
        }
        
        foreach (Product product in products)
        {
            if (product.StockQuantity == 0)
            {
                productsValidationErrors.Add(Result.Fail($"Product {product.Sku} has no stock."));
            }
        }
        if (productsValidationErrors.Any())
        {
            return new Error(string.Join(Environment.NewLine, Result.Merge(productsValidationErrors.ToResult()).Errors.Select(x => x.Message)));
        }
        var totalProductPrice = products.Sum(p => _priceHttpClient.GetPriceAsync(p.Sku).Result);
        var orderIdentifier = _orderIdentifierGenerator.Generate();
        
        var o = new Order
        {
            AccountId = request.AccountId,
            Total = totalProductPrice,
            Status = new OrderStatus(OrderStatusEnum.WaitingForPayment),
            Identifier = orderIdentifier,
            Products = products
        };
        
        var p = new CardPayment
        {
            Order = o,
            Status = new PaymentStatus(PaymentStatusEnum.Pending),
            Price = totalProductPrice,
            PaymentType = new PaymentType(PaymentTypeEnum.Card),
            CardNumber = request.VisaCard.CardNumber,
            Month = request.VisaCard.ExpirationMonth,
            Year = request.VisaCard.ExpirationYear,
            CVV = request.VisaCard.Cvv,
            Name = request.VisaCard.Owner            
        };
        
        _dbContext.Orders.Add(o);
        _dbContext.Payments.Add(p);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await SendEventAsync(cancellationToken, o, p);
        await transaction.CommitAsync(cancellationToken); // ✅ Ensures DB & Message are in sync

        return (orderIdentifier, o.Status);
    }

    private async Task SendEventAsync(CancellationToken cancellationToken, Order o, CardPayment p)
    {
        OrderPlaced orderPlaced = new(
            new OrderInfo(o.AccountId, DateTime.UtcNow, o.Identifier, o.Total, o.Status),
            new PaymentInfo(p.Status, p.PaymentType, p.Price, p.CardNumber, p.Month, p.Year, p.CVV, p.Name)
        );

        await _bus.Publish(orderPlaced, cancellationToken);
    }
}

