using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderJoe.MainSystem.Api.ViewModels.Requests;
using TraderJoe.MainSystem.Api.ViewModels.Responses;
using TraderJoe.MainSystem.TradingEngine.DataAccess;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(
    IOrderService orderService,
    IOrderRepository orderRepository) : ControllerBase
{
    [HttpGet("{symbol}/orders")]
    public async Task<IActionResult> GetOrdersBySymbol(string symbol)
    {
        var orders = await orderRepository.GetBySymbolAsync(symbol);
        return Ok(orders.Adapt<IEnumerable<OrderResponseVm>>());
    }
}
