namespace TraderJoe.MainSystem.TradingEngine.Models.Trading;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public string? RejectionReason { get; init; }

    public static ValidationResult Accept() => new() { IsValid = true };

    public static ValidationResult Reject(string reason) => new() { IsValid = false, RejectionReason = reason };
}
