using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

public class OrderEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid TradeRequestId { get; set; }

    [ForeignKey(nameof(TradeRequestId))]
    public TradeRequestEntity TradeRequest { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderSide Side { get; set; }
    public OrderType Type { get; set; }
    public OrderSource Source { get; set; }
    public DateTime CreatedAt { get; set; }
}
