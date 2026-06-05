using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.TradingEngine.Models.Order;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid TradeRequestId { get; init; }
    public string Symbol { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal Quantity { get; init; }
    public OrderSide Side { get; init; }
    public OrderType Type { get; init; }
    public OrderSource Source { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
