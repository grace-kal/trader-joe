namespace TraderJoe.MainSystem.Api.ViewModels.Requests;

public class UpdateTradingRulesVm
{
    public decimal MaxNotionalValue { get; set; }
    public decimal MaxQuantity { get; set; }
    public decimal MaxPriceDeviationPercent { get; set; }
    public bool IsDuplicateOrderIdCheckEnabled { get; set; }
    public bool IsSymbolWhitelistEnabled { get; set; }
    public List<string> SymbolWhitelist { get; set; } = new();
    public decimal AutoTradingSpreadThresholdPercent { get; set; }
}
