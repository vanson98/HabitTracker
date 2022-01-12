using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Overheads",
                table: "InvestmentChannels",
                newName: "TotalSellFee");

            migrationBuilder.AddColumn<decimal>(
                name: "AveragePrice",
                table: "Investments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "TotalBuyFee",
                table: "InvestmentChannels",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "TotalBuyFee",
                table: "InvestmentChannels");

            migrationBuilder.RenameColumn(
                name: "TotalSellFee",
                table: "InvestmentChannels",
                newName: "Overheads");
        }
    }
}
