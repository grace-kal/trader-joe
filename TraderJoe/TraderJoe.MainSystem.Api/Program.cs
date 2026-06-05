using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Threading.Channels;
using TraderJoe.MainSystem.Api.BackgroundServices;
using TraderJoe.MainSystem.TradingEngine;
using TraderJoe.MainSystem.TradingEngine.DataAccess;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Services;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;
using TraderJoe.PricingEngine.MarketDataSimulation;
using TraderJoe.PricingEngine.Publisher;
using TraderJoe.PricingEngine.Services;
using TraderJoe.PricingEngine.Services.Interfaces;
using TraderJoe.SharedNuget.Models;

var builder = WebApplication.CreateBuilder(args);

TraderJoe.MainSystem.Api.MappingConfig.Configure();
TraderJoe.MainSystem.TradingEngine.MappingConfig.Configure();

//channel- the pipe between PricingEngine and TradingService
//in production this would be Kafka/Service Bus/actual MB
var channel = Channel.CreateUnbounded<PriceUpdateEvent>();
builder.Services.AddSingleton(channel.Writer);
builder.Services.AddSingleton(channel.Reader);

//for this demo lightweight in-memory db is used
builder.Services.AddDbContext<TradingEngineDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("TradingDb")));
//builder.Services.AddDbContextFactory<TradingEngineDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("TradingDb")));

//Pricing Engine services
builder.Services.AddSingleton<IMarketDataFeed, SimulatedMarketDataFeed>();
builder.Services.AddSingleton<IPriceValidator, PriceValidator>();
builder.Services.AddSingleton<IPriceNormalizer, PriceNormalizer>();
builder.Services.AddSingleton<IPricePublisher, PricePublisher>();

//Trading Engine— in-memory store instead of Redis (singleton)
builder.Services.AddSingleton<IPriceStateStore, PriceStateStore>();

// Trading Engine— pure services (singleton, no I/O)
builder.Services.AddSingleton<ITradingRulesEngine, TradingRulesEngine>();
builder.Services.AddSingleton<IAutoTradingEngine, AutoTradingEngine>();

//Trading Engine— repositories (scoped)
builder.Services.AddScoped<IPriceStateRepository, PriceStateRepository>();
builder.Services.AddScoped<ITradeRequestRepository, TradeRequestRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRulesRepository, RulesRepository>();

//Trading Engine— services with DB (scoped)
builder.Services.AddSingleton<IRulesService, RulesService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPriceProcessingService, PriceProcessingService>();

//Background services
builder.Services.AddHostedService<PriceStateInitializerService>();
builder.Services.AddHostedService<PriceGeneratorHostedService>();
builder.Services.AddHostedService<PriceConsumerHostedService>();
builder.Services.AddHostedService<PriceStatePersistenceService>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TradingEngineDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
