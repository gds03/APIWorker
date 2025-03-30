using API.Features.Orders.GetOrders;
using API.Infrastructure.Hypermedia;
using Domain.ValueObjects;
using Domain.ValueObjects.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Orders.PlaceOrder;

public class PlaceOrderRequest
{
    public string? Sku { get; set; }
    public int Amount { get; set; }
}
public record PlaceOrderResponse(long OrderId, string Status) : ApiResponse;

[ApiController]
[Route("api/orders")]
public class PlaceOrderEndpoint : Controller
{
    private readonly IPlaceOrderHandler _placeOrderHandler;

    public PlaceOrderEndpoint(IPlaceOrderHandler placeOrderHandler)
    {
        _placeOrderHandler = placeOrderHandler;
    }
    
    [HttpPost("{accountId}/place", Name = "PlaceOrder")]
    public async Task<IActionResult> PlaceAsync(string accountId, [FromBody] PlaceOrderRequest request, CancellationToken ct)
    {
        var handlerRequest = PlaceOrderHandlerRequest.Create(accountId, request.Sku, request.Amount);
        if (handlerRequest.IsFailed)
        {
            return BadRequest(handlerRequest.Errors);
        }

        (BigInt orderId, OrderStatus status) = await _placeOrderHandler.HandleAsync(handlerRequest.Value, ct);
        PlaceOrderResponse orderResponse = new(orderId, status);
        orderResponse.Description = "Order placed successfully";
        orderResponse.Operations.Add(
            new Operation("GetOrders", HttpMethod.Get, Url.Action(nameof(GetOrdersEndpoint.GetAllAsync), nameof(GetOrdersEndpoint), new { accountId = accountId }))
        );
        
        return Ok(orderResponse);
    }
    
}