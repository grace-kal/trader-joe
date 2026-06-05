using System.Collections.Concurrent;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class PriceStateStore : IPriceStateStore
{
    private readonly ConcurrentDictionary<string, PriceState> _store = new();

    public void Update(PriceState state) => _store.AddOrUpdate(state.Symbol, state, (_, _) => state);

    public PriceState? Get(string symbol) => _store.TryGetValue(symbol, out var state) ? state : null;

    public IReadOnlyCollection<PriceState> GetAll() => _store.Values.ToList().AsReadOnly();
}