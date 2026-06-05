namespace TraderJoe.MainSystem.Api.ViewModels.Responses;

public class TradingRulesResponseVm
{
    public decimal MaxNotionalValue { get; set; }
    public decimal MaxQuantityPerOrder { get; set; }
    public decimal MaxPriceDeviationPercent { get; set; }
    public bool IsDuplicateOrderIdCheckEnabled { get; set; }
    public bool IsSymbolWhitelistEnabled { get; set; }
    public List<string> SymbolWhitelist { get; set; } = new();
    public decimal AutoTradingSpreadThresholdPercent { get; set; }
    public DateTime UpdatedAt { get; set; }
}
