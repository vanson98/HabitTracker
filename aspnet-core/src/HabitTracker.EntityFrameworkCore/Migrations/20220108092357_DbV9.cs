using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestingInfos");

            migrationBuilder.AddColumn<float>(
                name: "TotalFee",
                table: "Transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "Investments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InvestmentChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangnelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoneyInput = table.Column<float>(type: "real", nullable: false),
                    MoneyOutput = table.Column<float>(type: "real", nullable: false),
                    MoneyStock = table.Column<float>(type: "real", nullable: false),
                    MarketValueOfStocks = table.Column<float>(type: "real", nullable: false),
                    BuyFee = table.Column<float>(type: "real", nullable: false),
                    SellFee = table.Column<float>(type: "real", nullable: false),
                    Overheads = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentChannels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investments_ChannelId",
                table: "Investments",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_InvestmentChannels_ChannelId",
                table: "Investments",
                column: "ChannelId",
                principalTable: "InvestmentChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_InvestmentChannels_ChannelId",
                table: "Investments");

            migrationBuilder.DropTable(
                name: "InvestmentChannels");

            migrationBuilder.DropIndex(
                name: "IX_Investments_ChannelId",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "TotalFee",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Investments");

            migrationBuilder.CreateTable(
                name: "InvestingInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestmentChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketValueOfStocks = table.Column<float>(type: "real", nullable: false),
                    MoneyInput = table.Column<float>(type: "real", nullable: false),
                    MoneyOutput = table.Column<float>(type: "real", nullable: false),
                    NAV = table.Column<float>(type: "real", nullable: false),
                    PurchasingPower = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestingInfos", x => x.Id);
                });
        }
    }
}
