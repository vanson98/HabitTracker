using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class HabitV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TakeTime",
                table: "Habits",
                newName: "TimeGoal");

            migrationBuilder.RenameColumn(
                name: "Goal",
                table: "Habits",
                newName: "PracticeTime");

            migrationBuilder.RenameColumn(
                name: "PracticeTime",
                table: "HabitLogs",
                newName: "TimeLog");

            migrationBuilder.AddColumn<float>(
                name: "GoalPerDay",
                table: "Habits",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "HabitLogType",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLog",
                table: "HabitLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "HabitLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalPerDay",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "HabitLogType",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "DateLog",
                table: "HabitLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HabitLogs");

            migrationBuilder.RenameColumn(
                name: "TimeGoal",
                table: "Habits",
                newName: "TakeTime");

            migrationBuilder.RenameColumn(
                name: "PracticeTime",
                table: "Habits",
                newName: "Goal");

            migrationBuilder.RenameColumn(
                name: "TimeLog",
                table: "HabitLogs",
                newName: "PracticeTime");
        }
    }
}
