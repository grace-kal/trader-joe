namespace TraderJoe.PricingEngine.MarketDataSimulation.DataExample;

public static class SymbolBasePrices 
{
    public static readonly IReadOnlyDictionary<string, decimal> Prices = new Dictionary<string, decimal>
    {
        { "AAPL",   150m },
        { "GOOGL",  140m },
        { "MSFT",   380m },
        { "AMZN",   180m },
        { "TSLA",   250m },
        { "NVDA",   800m },
        { "EURUSD", 1.08m },
        { "GBPUSD", 1.27m },
        { "BTCUSD", 65000m },
        { "ETHUSD", 3500m }
    };
}
