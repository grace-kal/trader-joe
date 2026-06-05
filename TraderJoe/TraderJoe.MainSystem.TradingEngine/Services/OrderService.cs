using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using TraderJoe.MainSystem.TradingEngine.Models.Order;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class OrderService(
    ITradingRulesEngine rulesEngine,
    IRulesService rulesService,
    IPriceStateStore priceStateStore,
    ITradeRequestRepository tradeRequestRepository,
    IOrderRepository orderRepository,
    TradingEngineDbContext db) : IOrderService
{
    public async Task<TradeRequest> ProcessAsync(TradeRequest request)
    {
        //check if price for symbol exists
        var symbolPriceState = priceStateStore.Get(request.Symbol);
        if (symbolPriceState is null)
        {
            request.Status = OrderStatus.Rejected;
            request.RejectionReason = $"No price data available for symbol {request.Symbol}.";
            await PersistAsync(request, createOrder: false);
            return request;
        }

        //validate against trading rules
        var rules = await rulesService.GetCurrentRulesAsync();

        if (rules.IsDuplicateOrderIdCheckEnabled && request.IdempotencyKey.HasValue)
        {
            var existing = await tradeRequestRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey.Value);
            if (existing is not null)
                return existing.Adapt<TradeRequest>();
        }

        var validation = rulesEngine.Validate(request, symbolPriceState, rules);
        (request.Status, request.RejectionReason) = validation.IsValid ? (OrderStatus.Accepted, null) : (OrderStatus.Rejected, validation.RejectionReason);

        await PersistAsync(request, createOrder: validation.IsValid);
        return request;
    }

    private async Task PersistAsync(TradeRequest request, bool createOrder)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            await tradeRequestRepository.AddAsync(request.Adapt<TradeRequestEntity>());

            if (createOrder)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    TradeRequestId = request.Id,
                    Symbol = request.Symbol,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    Side = request.Side,
                   Type = request.Type,
                    Source = request.Source,
                    CreatedAt = DateTime.UtcNow
                };
                await orderRepository.AddAsync(order.Adapt<OrderEntity>());
            }

            await db.SaveChangesAsync(); 
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
