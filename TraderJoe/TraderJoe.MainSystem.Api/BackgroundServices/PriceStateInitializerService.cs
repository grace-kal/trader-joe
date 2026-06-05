using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.BackgroundServices;

public class PriceStateInitializerService(
    IServiceScopeFactory scopeFactory,
    IPriceStateStore priceStateStore,
    ILogger<PriceStateInitializerService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        logger.LogInformation("Loading price states from database into memory...");

        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IPriceStateRepository>();

        var entities = await repo.GetAllAsync();
        var count = 0;

        foreach (var entity in entities)
        {
            var state = entity.Adapt<PriceState>();
            priceStateStore.Update(state);
            count++;
        }

        logger.LogInformation("Loaded {Count} price states into memory", count);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
