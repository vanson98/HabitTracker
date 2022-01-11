using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSell",
                table: "Investments",
                newName: "TotalMoneySell");

            migrationBuilder.RenameColumn(
                name: "TotalBuy",
                table: "Investments",
                newName: "TotalMoneyBuy");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Investments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TotalAmountBuy",
                table: "Investments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalAmountSell",
                table: "Investments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "TotalAmountBuy",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "TotalAmountSell",
                table: "Investments");

            migrationBuilder.RenameColumn(
                name: "TotalMoneySell",
                table: "Investments",
                newName: "TotalSell");

            migrationBuilder.RenameColumn(
                name: "TotalMoneyBuy",
                table: "Investments",
                newName: "TotalBuy");
        }
    }
}
