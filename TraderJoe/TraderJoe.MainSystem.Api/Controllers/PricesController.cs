using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderJoe.MainSystem.Api.ViewModels.Responses;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PricesController(IPriceStateStore priceStateStore) : ControllerBase
{
    [HttpGet("{symbol}")]
    public IActionResult GetLatestPrice(string symbol)
    {
        var state = priceStateStore.Get(symbol.ToUpper());

        if (state is null)
            return NotFound($"No price data available for symbol {symbol}.");

        return Ok(state.Adapt<PriceStateResponseVm>());
    }
}
