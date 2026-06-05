using System.Threading.Channels;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.MainSystem.Api.BackgroundServices;

// in production this would be a separate microservice
// hosted here because in-memory Channel requires producer and consumer in same process
public class PriceConsumerHostedService(
    ChannelReader<PriceUpdateEvent> reader,
    IServiceScopeFactory scopeFactory,
    ILogger<PriceConsumerHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Price consumer started — waiting for price updates");

        await foreach (var priceUpdate in reader.ReadAllAsync(ct))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var priceProcessingService = scope.ServiceProvider.GetRequiredService<IPriceProcessingService>();

                await priceProcessingService.ProcessPriceUpdateAsync(priceUpdate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error processing price update for {Symbol}",
                    priceUpdate.Symbol);
            }
        }
    }
}