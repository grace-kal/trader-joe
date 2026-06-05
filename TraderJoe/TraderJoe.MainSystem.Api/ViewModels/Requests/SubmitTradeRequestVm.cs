using System.ComponentModel.DataAnnotations;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;

namespace TraderJoe.MainSystem.Api.ViewModels.Requests;

public class SubmitTradeRequestVm
{
    public Guid? IdempotencyKey { get; set; }
    public string Symbol { get; set; } = string.Empty;

    [Range(0.0001, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public decimal Quantity { get; set; }
    public OrderSide Side { get; set; }
    public OrderType OrderType { get; set; } = OrderType.Limit;
}