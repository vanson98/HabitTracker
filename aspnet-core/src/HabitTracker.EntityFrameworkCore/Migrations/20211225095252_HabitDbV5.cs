using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class HabitDbV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "GoalTime",
                table: "HabitCategories",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalTime",
                table: "HabitCategories");
        }
    }
}
