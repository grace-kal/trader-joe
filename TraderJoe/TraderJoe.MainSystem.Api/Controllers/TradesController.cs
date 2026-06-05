using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderJoe.MainSystem.Api.ViewModels.Requests;
using TraderJoe.MainSystem.Api.ViewModels.Responses;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TradesController(
    IOrderService orderService,
    ITradeRequestRepository tradeRequestRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SubmitTradeRequest([FromBody] SubmitTradeRequestVm vm)
    {
        var request = vm.Adapt<TradeRequest>();
        var result = await orderService.ProcessAsync(request);
        var response = result.Adapt<TradeRequestResponseVm>();

        return result.Status == OrderStatus.Accepted
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetTradeHistory(
        [FromQuery] string? symbol,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? status)
    {
        var results = await tradeRequestRepository.GetFilteredAsync(symbol, from, to, status);

        return Ok(results.Adapt<IEnumerable<TradeRequestResponseVm>>());
    }
}
