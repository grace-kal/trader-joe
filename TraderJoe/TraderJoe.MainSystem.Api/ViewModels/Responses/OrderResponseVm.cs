using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.Api.ViewModels.Responses;

public class OrderResponseVm
{
    public Guid Id { get; set; }
    public Guid TradeRequestId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderSide Side { get; set; }
    public OrderType OrderType { get; set; }
    public OrderSource Source { get; set; }
    public DateTime CreatedAt { get; set; }
}
