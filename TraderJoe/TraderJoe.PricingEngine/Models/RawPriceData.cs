using System;
using System.Collections.Generic;
using System.Text;

namespace TraderJoe.PricingEngine.Models;

public class RawPriceData
{
    public string Symbol { get; init; } = string.Empty;
    public decimal BidPrice { get; init; }
    public decimal AskPrice { get; init; }
    public DateTime Timestamp { get; init; }
}
