using API.Features.Orders.PlaceOrder;
using API.Infrastructure.Hypermedia;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Orders.GetOrders;

[ApiController]
[Route("api/orders")]
public class GetOrdersEndpoint : Controller
{
    private readonly IGetOrdersHandler _getOrdersHandler;

    public GetOrdersEndpoint(IGetOrdersHandler getOrdersHandler)
    {
        _getOrdersHandler = getOrdersHandler;
    }
    
    [HttpGet("{accountId}", Name = "GetOrders")]
    public async Task<IActionResult> GetAllAsync(string accountId, CancellationToken ct)
    {
        var handlerRequest = GetOrdersHandlerRequest.Create(accountId);
        if (handlerRequest.IsFailed)
        {
            return BadRequest(handlerRequest.Errors);
        }
        
        var handlerResponse = await _getOrdersHandler.HandleAsync(handlerRequest.Value, ct);
        return Ok(new GetAllResponse(handlerResponse.Orders)
        {
            Operations =
            {
                new("PlaceOrder", HttpMethod.Post, Url.Action(nameof(PlaceOrderEndpoint.PlaceAsync), nameof(PlaceOrderEndpoint), new { accountId = accountId}))
            }
        });
    }
}

public record GetAllResponse(List<GetOrdersHandlerOrderResponse> Orders) : ApiResponse;