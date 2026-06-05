using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class RulesService(IRulesRepository rulesRepository) : IRulesService
{
    public async Task<TradingRules> GetCurrentRulesAsync()
    {
        var entity = await rulesRepository.GetCurrentAsync();

        return entity is null
            ? GetDefaultRules()
            : entity.Adapt<TradingRules>();
    }

    public async Task<TradingRules> UpdateRulesAsync(TradingRules rules)
    {
        var existing = await rulesRepository.GetCurrentAsync();

        if (existing is null)
        {
            var entity = rules.Adapt<TradingRulesEntity>();
            await rulesRepository.AddAsync(entity);
            await rulesRepository.SaveChangesAsync();
            return entity.Adapt<TradingRules>();
        }

        rules.Adapt(existing);
        existing.UpdatedAt = DateTime.UtcNow;

        await rulesRepository.UpdateAsync(existing);
        await rulesRepository.SaveChangesAsync();

        return existing.Adapt<TradingRules>();
    }

    private static TradingRules GetDefaultRules() =>
        new()
        {
            MaxNotionalValue = 1_000_000m,
            MaxQuantityPerOrder = 10_000m,
            MaxPriceDeviationPercent = 0.8m,
            IsDuplicateOrderIdCheckEnabled = false,
            IsSymbolWhitelistEnabled = false,
            SymbolWhitelist = new List<string>(),
            AutoTradingSpreadThresholdPercent = 0.5m
        };
}
