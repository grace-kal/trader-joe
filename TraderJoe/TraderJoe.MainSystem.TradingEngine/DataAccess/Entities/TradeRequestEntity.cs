using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

[Index(nameof(IdempotencyKey))]
public class TradeRequestEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid? IdempotencyKey { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderType Type { get; set; }
    public OrderSide Side { get; set; }
    public OrderSource Source { get; set; }
    public OrderStatus Status { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    public DateTime SubmittedAt { get; set; }
}
