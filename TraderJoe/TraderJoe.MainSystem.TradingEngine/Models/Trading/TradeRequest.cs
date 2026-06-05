using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.TradingEngine.Models.Trading;

public class TradeRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid? IdempotencyKey { get; init; }
    public string Symbol { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal Quantity { get; init; }
    public OrderType Type { get; init; }
    public OrderSide Side { get; init; }
    public OrderSource Source { get; init; }
    public OrderStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime SubmittedAt { get; init; } = DateTime.UtcNow;
}
