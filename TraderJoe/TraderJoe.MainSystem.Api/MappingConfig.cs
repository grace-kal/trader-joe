using Mapster;
using TraderJoe.MainSystem.Api.ViewModels.Requests;
using TraderJoe.MainSystem.Api.ViewModels.Responses;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using TraderJoe.MainSystem.TradingEngine.Models.Order;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.Api;

public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<SubmitTradeRequestVm, TradeRequest>
            .NewConfig()
            .Map(dest => dest.Source, _ => OrderSource.Api)
            .Map(dest => dest.Symbol, src => src.Symbol.ToUpper().Trim()); ;

        TypeAdapterConfig<TradeRequest, TradeRequestResponseVm>
            .NewConfig();

        TypeAdapterConfig<Order, OrderResponseVm>
            .NewConfig();

        TypeAdapterConfig<PriceState, PriceStateResponseVm>
            .NewConfig();

        TypeAdapterConfig<TradingRules, TradingRulesResponseVm>
            .NewConfig();

        TypeAdapterConfig<UpdateTradingRulesVm, TradingRules>
            .NewConfig();
    }
}
