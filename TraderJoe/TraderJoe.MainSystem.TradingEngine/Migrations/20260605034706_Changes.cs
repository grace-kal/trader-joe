using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraderJoe.MainSystem.TradingEngine.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderType",
                table: "TradeRequests",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "SymbolPriceStates",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "OrderType",
                table: "Orders",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TradeRequests",
                newName: "OrderType");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "SymbolPriceStates",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Orders",
                newName: "OrderType");
        }
    }
}
