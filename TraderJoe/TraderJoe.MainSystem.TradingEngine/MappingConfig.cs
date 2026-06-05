using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.Models.Order;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.TradingEngine;

public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<TradingRulesEntity, TradingRules>
            .NewConfig()
            .Map(dest => dest.SymbolWhitelist,
                 src => string.IsNullOrWhiteSpace(src.SymbolWhitelist)
                     ? new List<string>()
                     : src.SymbolWhitelist.Split(',').ToList());

        TypeAdapterConfig<TradingRules, TradingRulesEntity>
            .NewConfig()
            .Map(dest => dest.SymbolWhitelist,
                 src => string.Join(",", src.SymbolWhitelist));

        TypeAdapterConfig<TradeRequestEntity, TradeRequest>
            .NewConfig();

        TypeAdapterConfig<TradeRequest, TradeRequestEntity>
            .NewConfig();

        TypeAdapterConfig<OrderEntity, Order>
            .NewConfig();

        TypeAdapterConfig<Order, OrderEntity>
            .NewConfig();

        TypeAdapterConfig<SymbolPriceStateEntity, PriceState>
            .NewConfig();

        TypeAdapterConfig<PriceState, SymbolPriceStateEntity>
            .NewConfig();
    }
}
