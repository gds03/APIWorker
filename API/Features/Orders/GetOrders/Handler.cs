using API.Infrastructure;
using Domain.Database;
using Domain.ValueObjects.Account;
using FluentResults;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Features.Orders.GetOrders;

public class GetOrdersHandlerRequest
{
    private GetOrdersHandlerRequest() {}
    public AccountId AccountId { get; private set; }

    public static Result<GetOrdersHandlerRequest> Create(string? accountId)
    {
        Result<AccountId> voAccountId = AccountId.Create(accountId);
        return voAccountId.IsFailed
            ? Result.Fail<GetOrdersHandlerRequest>(voAccountId.Errors)
            : Result.Ok(new GetOrdersHandlerRequest
            {
                AccountId = voAccountId.Value
            });
    }
}

public record GetOrdersHandlerResponse(AccountId AccountId, List<GetOrdersHandlerOrderResponse> Orders);

public class GetOrdersHandlerOrderResponse
{
    public string Status { get; init; }
    public DateTime UtcPlacedAt { get; init;}
    public decimal Total { get;init; }
}

public interface IGetOrdersHandler : IHandler
{
    Task<GetOrdersHandlerResponse> HandleAsync(GetOrdersHandlerRequest request, CancellationToken cancellationToken);
}

public class GetOrdersHandler : IGetOrdersHandler
{
    private readonly ILogger<GetOrdersHandler> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IBus _bus;

    public GetOrdersHandler(ILogger<GetOrdersHandler> logger, AppDbContext dbContext, IBus bus)
    {
        _logger = logger;
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task<GetOrdersHandlerResponse> HandleAsync(GetOrdersHandlerRequest request, CancellationToken cancellationToken)
    {
        var orders = await _dbContext.Orders.ToListAsync(cancellationToken);
        
        // go to the db and read the orders for this account.
        return new(request.AccountId, orders.Select(x => new GetOrdersHandlerOrderResponse
        {
            Status = x.Status,
            Total = x.Total,
            UtcPlacedAt = x.CreatedWhenUtc
        }).ToList());
    }
}