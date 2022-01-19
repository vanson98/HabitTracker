using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketValueOfStocks",
                table: "InvestmentChannels");

            migrationBuilder.RenameColumn(
                name: "CurrentPrice",
                table: "Investments",
                newName: "MarketPrice");

            migrationBuilder.RenameColumn(
                name: "AveragePrice",
                table: "Investments",
                newName: "CapitalCost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarketPrice",
                table: "Investments",
                newName: "CurrentPrice");

            migrationBuilder.RenameColumn(
                name: "CapitalCost",
                table: "Investments",
                newName: "AveragePrice");

            migrationBuilder.AddColumn<float>(
                name: "MarketValueOfStocks",
                table: "InvestmentChannels",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
