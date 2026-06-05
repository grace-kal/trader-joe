using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

//enforce the tradinf rules
public class TradingRulesEngine : ITradingRulesEngine
{
    public ValidationResult Validate(TradeRequest request, PriceState priceState, TradingRules rules)
    {
        //lower boundary checks
        if (request.Price <= 0)
            return ValidationResult.Reject("Price must be greater than zero.");

        if (request.Quantity <= 0)
            return ValidationResult.Reject("Quantity must be greater than zero.");

        // whitelist check for symbol
        if (rules.IsSymbolWhitelistEnabled && !rules.SymbolWhitelist.Contains(request.Symbol, StringComparer.OrdinalIgnoreCase))
            return ValidationResult.Reject($"Symbol {request.Symbol} is not in the whitelist.");

        // max quantity check per order
        if (request.Quantity > rules.MaxQuantityPerOrder)
            return ValidationResult.Reject($"Quantity {request.Quantity} exceeds maximum allowed {rules.MaxQuantityPerOrder}.");

        // max notinal amount per order
        var notional = request.Price * request.Quantity;
        if (notional > rules.MaxNotionalValue)
            return ValidationResult.Reject($"Notional value {notional} exceeds maximum allowed {rules.MaxNotionalValue}.");

        // price deviation check
        var deviation = Math.Abs(request.Price - priceState.CurrentMarketPrice) / priceState.CurrentMarketPrice * 100;
        if (deviation > rules.MaxPriceDeviationPercent)
            return ValidationResult.Reject($"Price deviation {deviation:F4}% exceeds maximum allowed {rules.MaxPriceDeviationPercent}%.");

        return ValidationResult.Accept();
    }
}
