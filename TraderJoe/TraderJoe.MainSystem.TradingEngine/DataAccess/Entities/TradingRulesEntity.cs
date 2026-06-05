using System.ComponentModel.DataAnnotations;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

//would be a config in like Azure Storage configs in real scenario, for simplicity using sql here, thats why whitelist looks weird :)
public class TradingRulesEntity
{
    [Key]
    public Guid Id { get; set; }
    public decimal MaxNotionalValue { get; set; }
    public decimal MaxQuantityPerOrder { get; set; }
    public decimal MaxPriceDeviationPercent { get; set; }
    public bool IsDuplicateOrderIdCheckEnabled { get; set; }
    public bool IsSymbolWhitelistEnabled { get; set; }

    [MaxLength(1000)]
    public string SymbolWhitelist { get; set; } = string.Empty;

    public decimal AutoTradingSpreadThresholdPercent { get; set; }
    public DateTime UpdatedAt { get; set; }
}
