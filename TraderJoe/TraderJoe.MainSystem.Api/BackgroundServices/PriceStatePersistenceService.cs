using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.BackgroundServices;

// periodically flushes latest price states from memory to DB
// in production Redis would handle this entirely
public class PriceStatePersistenceService(
    IPriceStateStore priceStateStore,
    IServiceScopeFactory scopeFactory,
    ILogger<PriceStatePersistenceService> logger) : BackgroundService
{
    private readonly TimeSpan _flushInterval = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Price state persistence service started — flushing every {Seconds}s",_flushInterval.TotalSeconds);

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_flushInterval, ct);

            try
            {
                await FlushAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error flushing price states to DB");
            }
        }
    }

    private async Task FlushAsync()
    {
        var states = priceStateStore.GetAll();
        if (!states.Any()) return;

        using var scope = scopeFactory.CreateScope();
        var priceStateRepository = scope.ServiceProvider.GetRequiredService<IPriceStateRepository>();


        foreach (var state in states)
        {
            var entity = state.Adapt<SymbolPriceStateEntity>();
            await priceStateRepository.UpsertAsync(entity);
        }
        await priceStateRepository.SaveChangesAsync();

        logger.LogInformation("Flushed {Count} price states to DB", states.Count);
    }
}
