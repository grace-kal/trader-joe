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
    IOrderRepository orderRepository) : IOrderService
{
    public async Task<TradeRequest> ProcessAsync(TradeRequest request)
    {
        //check if price for symbol exists
        var symbolPriceState = priceStateStore.Get(request.Symbol);
        if (symbolPriceState is null)
        {
            request.Status = OrderStatus.Rejected;
            request.RejectionReason = $"No price data available for symbol {request.Symbol}.";
            await CreateTradeRequestAsync(request);
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

        await CreateTradeRequestAsync(request);

        if (validation.IsValid)
            await CreateOrderAsync(request);

        return request;
    }

    private async Task CreateTradeRequestAsync(TradeRequest request)
    {
        var entity = request.Adapt<TradeRequestEntity>();
        await tradeRequestRepository.AddAsync(entity);
        await tradeRequestRepository.SaveChangesAsync();
    }

    private async Task CreateOrderAsync(TradeRequest request)
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

        var entity = order.Adapt<OrderEntity>();
        await orderRepository.AddAsync(entity);
        await orderRepository.SaveChangesAsync();
    }
}
