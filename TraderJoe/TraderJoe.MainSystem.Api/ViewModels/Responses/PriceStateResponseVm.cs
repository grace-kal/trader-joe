namespace TraderJoe.MainSystem.Api.ViewModels.Responses;

public class PriceStateResponseVm
{
    public string Symbol { get; set; } = string.Empty;
    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }
    public decimal CurrentMarketPrice { get; set; }
    public decimal Spread { get; set; }
    public decimal SpreadPercent { get; set; }
    public DateTime EventTimestamp { get; set; }
    public DateTime UpdatedAt { get; set; }
}
