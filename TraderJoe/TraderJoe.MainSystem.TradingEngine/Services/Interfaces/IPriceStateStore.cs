using TraderJoe.MainSystem.TradingEngine.Models.Price;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

//this would be REdis in real scenario
public interface IPriceStateStore
{
    void Update(PriceState state);
    PriceState? Get(string symbol);
    IReadOnlyCollection<PriceState> GetAll();
}