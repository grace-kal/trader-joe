using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

public interface IRulesService
{
    Task<TradingRules> GetCurrentRulesAsync();
    Task<TradingRules> UpdateRulesAsync(TradingRules rules);
}
