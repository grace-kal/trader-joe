namespace TraderJoe.MainSystem.TradingEngine.Models.Trading;

public class TradingRules
{
    public Guid Id { get; set; }
    public decimal MaxNotionalValue { get; set; }
    public decimal MaxQuantityPerOrder { get; set; }
    public decimal MaxPriceDeviationPercent { get; set; } = 0.8m;

    public bool IsDuplicateOrderIdCheckEnabled { get; set; }
    public bool IsSymbolWhitelistEnabled { get; set; }
    public List<string> SymbolWhitelist { get; set; } = new();

    public decimal AutoTradingSpreadThresholdPercent { get; set; }

    public DateTime UpdatedAt { get; set; }
}
