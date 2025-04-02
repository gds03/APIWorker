using API.Features._Shared.Endpoints;
using API.Features.Orders.GetOrders;
using API.Infrastructure.Hypermedia;
using Domain.ValueObjects;
using Domain.ValueObjects.Order;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace API.Features.Orders.PlaceOrder;

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
        var handlerRequest = PlaceOrderHandlerRequest.Create(
            accountId, 
            request.Products.Select(p => (p.Sku, p.Amount)),
            (request.VisaPayment.CardNumber, request.VisaPayment.ExpirationMonth, request.VisaPayment.ExpirationYear, request.VisaPayment.Cvv)
        );
        
        if (handlerRequest.IsFailed)
        {
            return BadRequest(handlerRequest.Errors);
        }

        OneOf<(BigInt orderId, OrderStatus orderStatus), Error> result = await _placeOrderHandler.HandleAsync(handlerRequest.Value, ct);
        if (result.IsT1)
        {
            return BadRequest(result.AsT1);
        }

        var handlerResponse = result.AsT0;
        
        PlaceOrderResponse orderResponse = new(handlerResponse.orderId, handlerResponse.orderStatus);
        orderResponse.Description = "Order placed successfully";
        orderResponse.Operations.Add(
            new Operation("GetOrders", HttpMethod.Get, Url.Action(nameof(GetOrdersEndpoint.GetAllAsync), nameof(GetOrdersEndpoint), new { accountId = accountId }))
        );
        
        return Ok(orderResponse);
    }
}

public class PlaceOrderRequest
{
    public VisaCardPaymentRequest VisaPayment { get; set; }
    public List<ProductAmountRequest> Products { get; set; } = [];
}
public record PlaceOrderResponse(long OrderIdentifier, string OrderStatus) : ApiResponse;