using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderJoe.MainSystem.Api.ViewModels.Requests;
using TraderJoe.MainSystem.Api.ViewModels.Responses;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RulesController(IRulesService rulesService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRules()
    {
        var rules = await rulesService.GetCurrentRulesAsync();
        return Ok(rules.Adapt<TradingRulesResponseVm>());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRules([FromBody] UpdateTradingRulesVm vm)
    {
        var rules = vm.Adapt<TradingRules>();
        var updated = await rulesService.UpdateRulesAsync(rules);
        return Ok(updated.Adapt<TradingRulesResponseVm>());
    }
}
