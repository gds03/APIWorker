using API.HttpClients;
using API.Infrastructure;
using Domain.Database;
using Domain.ValueObjects;
using Domain.ValueObjects.Account;
using Domain.ValueObjects.Order;
using Domain.ValueObjects.Product;
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

    public static Result<PlaceOrderHandlerRequest> Create(string? accountId, IEnumerable<(string? sku, int amount)> products)
    {
        List<Result> results = [];
        Result<AccountId> voAccountId = AccountId.Create(accountId);
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
        var mergedResult = Result.Merge(results.ToResult());
        if (mergedResult.IsFailed)
        {
            return Result.Fail(mergedResult.Errors);
        }

        return Result.Ok(new PlaceOrderHandlerRequest
        {
            AccountId = voAccountId.Value,
            Products = validatedProducts
        });
    }
}
public interface IPlaceOrderHandler : IHandler
{
    Task<OneOf<(BigInt orderId, OrderStatus orderStatus), Error>> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken);
}

public class PlaceOrderHandler : IPlaceOrderHandler
{
    private readonly ILogger<PlaceOrderHandler> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IBus _bus;
    private readonly IPriceHttpClient _priceHttpClient;
    private readonly IOrderIdentifierGenerator _orderIdentifierGenerator;

    public PlaceOrderHandler(
        ILogger<PlaceOrderHandler> logger,
        AppDbContext dbContext,
        IBus bus,
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

    public async Task<OneOf<(BigInt orderId, OrderStatus orderStatus), Error>> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken)
    {
        // Outbox pattern now.
        var skus = request.Products.Select(x => x.sku).ToArray();

        var productsValidationErrors = new List<Result>();
        var products = await _dbContext.Products.Where(p => skus.Any(sku => sku == p.Sku)).ToArrayAsync(cancellationToken);
        foreach (Product p in products)
        {
            if (p.StockQuantity == 0)
            {
                productsValidationErrors.Add(Result.Fail($"Product {p.Sku} has no stock."));
            }
        }
        if (productsValidationErrors.Any())
        {
            return new Error(string.Join(Environment.NewLine, Result.Merge(productsValidationErrors.ToResult()).Errors.Select(x => x.Message)));
        }

        var totalProductPrice = products.Sum(p => _priceHttpClient.GetPriceAsync(p.Sku).Result);
        var orderGenId = _orderIdentifierGenerator.Generate();
        var order = new Order
        {
            AccountId = request.AccountId,
            Total = totalProductPrice,
            Status = new OrderStatus(OrderStatusEnum.WaitingForPayment),
            Identifier = orderGenId,
            Products = products,
            Payment = new Payment
            {
                
            }
        };
        
        return (order.Id, order.Status);
    }
}

