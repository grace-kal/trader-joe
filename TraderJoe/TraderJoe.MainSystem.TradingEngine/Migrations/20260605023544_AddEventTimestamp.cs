using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraderJoe.MainSystem.TradingEngine.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventTimestamp",
                table: "SymbolPriceStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_IdempotencyKey",
                table: "TradeRequests",
                column: "IdempotencyKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TradeRequests_IdempotencyKey",
                table: "TradeRequests");

            migrationBuilder.DropColumn(
                name: "EventTimestamp",
                table: "SymbolPriceStates");
        }
    }
}
