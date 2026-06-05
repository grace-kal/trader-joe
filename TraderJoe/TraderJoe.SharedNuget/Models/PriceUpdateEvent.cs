namespace TraderJoe.SharedNuget.Models;

public class PriceUpdateEvent
{
    public string Symbol { get; init; } = string.Empty;
    public decimal BidPrice { get; init; }
    public decimal AskPrice { get; init; }
    public DateTime EventTimestamp { get; init; }
}