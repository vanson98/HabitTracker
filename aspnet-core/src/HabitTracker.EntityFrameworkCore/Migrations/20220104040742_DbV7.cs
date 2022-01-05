using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Transactions",
                newName: "TransactionTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionTime",
                table: "Transactions",
                newName: "CreatedDate");
        }
    }
}
