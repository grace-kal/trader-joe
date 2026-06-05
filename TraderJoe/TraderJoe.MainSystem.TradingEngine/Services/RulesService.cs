using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TraderJoe.MainSystem.TradingEngine.DataAccess;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class RulesService(IServiceScopeFactory scopeFactory) : IRulesService
{
    private record CachedRules(TradingRules Rules, DateTime Expiry);
    private volatile CachedRules? _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public async Task<TradingRules> GetCurrentRulesAsync()
    {
        var cached = _cache;
        if (cached is not null && DateTime.UtcNow < cached.Expiry)
            return cached.Rules;

        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IRulesRepository>();

        var entity = await repo.GetCurrentAsync();
        var rules = entity is null ? GetDefaultRules() : entity.Adapt<TradingRules>();

        _cache = new CachedRules(rules, DateTime.UtcNow.Add(_cacheDuration));
        return rules;
    }

    public async Task<TradingRules> UpdateRulesAsync(TradingRules rules)
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IRulesRepository>();

        var existing = await repo.GetCurrentAsync();

        TradingRules result;
        if (existing is null)
        {
            var entity = rules.Adapt<TradingRulesEntity>();
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            result = entity.Adapt<TradingRules>();
        }
        else
        {
            rules.Adapt(existing);
            existing.UpdatedAt = DateTime.UtcNow;
            await repo.UpdateAsync(existing);
            await repo.SaveChangesAsync();
            result = existing.Adapt<TradingRules>();
        }

        // refresh cache immediately so the runtime update is visible at once
        _cache = new CachedRules(result, DateTime.UtcNow.Add(_cacheDuration));
        return result;
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
            AutoTradingSpreadThresholdPercent = 0.2m,
            UpdatedAt = DateTime.UtcNow
        };
}
