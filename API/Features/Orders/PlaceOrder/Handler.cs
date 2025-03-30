using API.Infrastructure;
using Domain.ValueObjects;
using Domain.ValueObjects.Account;
using Domain.ValueObjects.Orders;
using Domain.ValueObjects.Products;
using FluentResults;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace API.Features.Orders.PlaceOrder;
public class PlaceOrderHandlerRequest
{
    private PlaceOrderHandlerRequest(){ }

    public AccountId AccountId { get; private set; }
    public Sku Sku { get; private set; }
    public BigInt Amount { get; private set; }

    public static Result<PlaceOrderHandlerRequest> Create(string? accountId, string? sku, int amount)
    {
        Result<AccountId> voAccountId = AccountId.Create(accountId);
        Result<Sku> voSku = Sku.Create(sku);
        Result<BigInt> voAmount = BigInt.Create(amount);
        
        Result result = Result.Merge(voAccountId, voSku, voAmount);
        if (result.IsFailed)
        {
            return Result.Fail<PlaceOrderHandlerRequest>(result.Errors);
        }

        return Result.Ok(new PlaceOrderHandlerRequest
        {
            AccountId = voAccountId.Value,
            Sku = voSku.Value,
            Amount = voAmount.Value
        });
    }
}
public interface IPlaceOrderHandler : IHandler
{
    Task<(BigInt orderId, OrderStatus orderStatus)> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken);
}

public class PlaceOrderHandler : IPlaceOrderHandler
{
    private readonly ILogger<PlaceOrderHandler> _logger;
    private readonly IBus _bus;

    public PlaceOrderHandler(ILogger<PlaceOrderHandler> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task<(BigInt orderId, OrderStatus orderStatus)> HandleAsync(PlaceOrderHandlerRequest request, CancellationToken cancellationToken)
    {
        // Outbox pattern now.
        
        // persist into DB, return ID
        BigInt orderId = (BigInt)new Random().NextInt64();
        OrderStatus status = new OrderStatus(OrderStatusEnum.Pending);
        
        // send message to BUS
        // await _bus.Publish(new OrderPlaced(orderId, status, request.Sku, request.Amount, DateTime.UtcNow));
        return (orderId, status);
    }
}

