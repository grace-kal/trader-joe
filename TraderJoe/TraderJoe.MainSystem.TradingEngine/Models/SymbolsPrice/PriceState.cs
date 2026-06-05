namespace TraderJoe.MainSystem.TradingEngine.Models.Price;

public class PriceState
{
    public string Symbol { get; set; } = string.Empty;
    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }
    public decimal CurrentMarketPrice { get; set; }
    public decimal Spread { get; set; }
    public decimal SpreadPercent { get; set; }

    // for auto trading determening
    public decimal? PreviousMarketPrice { get; set; }

    public DateTime EventTimestamp { get; set; }
    public DateTime UpdatedAt { get; set; }
}
