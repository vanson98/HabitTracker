using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class DbV8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Transactions",
                newName: "LastModificationTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Transactions",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Transactions",
                newName: "UpdateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }
    }
}
