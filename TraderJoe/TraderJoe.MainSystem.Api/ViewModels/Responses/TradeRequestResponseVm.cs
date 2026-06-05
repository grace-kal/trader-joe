using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.Api.ViewModels.Responses;

public class TradeRequestResponseVm
{
    public Guid Id { get; set; }
    public Guid? IdempotencyKey { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderSide Side { get; set; }
    public OrderType OrderType { get; set; }
    public OrderSource Source { get; set; }
    public OrderStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime SubmittedAt { get; set; }
}
