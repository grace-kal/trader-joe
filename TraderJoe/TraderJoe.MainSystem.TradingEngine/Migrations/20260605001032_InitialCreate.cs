using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraderJoe.MainSystem.TradingEngine.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SymbolPriceStates",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    BidPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    AskPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrentMarketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Spread = table.Column<decimal>(type: "TEXT", nullable: false),
                    SpreadPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    PreviousMarketPrice = table.Column<decimal>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolPriceStates", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "TradeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdempotencyKey = table.Column<Guid>(type: "TEXT", nullable: true),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderType = table.Column<int>(type: "INTEGER", nullable: false),
                    Side = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    RejectionReason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradingRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxNotionalValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxQuantityPerOrder = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxPriceDeviationPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsDuplicateOrderIdCheckEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSymbolWhitelistEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SymbolWhitelist = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    AutoTradingSpreadThresholdPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TradeRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Side = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderType = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_TradeRequests_TradeRequestId",
                        column: x => x.TradeRequestId,
                        principalTable: "TradeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TradeRequestId",
                table: "Orders",
                column: "TradeRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "SymbolPriceStates");

            migrationBuilder.DropTable(
                name: "TradingRules");

            migrationBuilder.DropTable(
                name: "TradeRequests");
        }
    }
}
