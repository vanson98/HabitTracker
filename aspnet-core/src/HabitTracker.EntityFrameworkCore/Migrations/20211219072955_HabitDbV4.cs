using Microsoft.EntityFrameworkCore.Migrations;

namespace HabitTracker.Migrations
{
    public partial class HabitDbV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Habits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HabitCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habits_CategoryId",
                table: "Habits",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_HabitCategories_CategoryId",
                table: "Habits",
                column: "CategoryId",
                principalTable: "HabitCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_HabitCategories_CategoryId",
                table: "Habits");

            migrationBuilder.DropTable(
                name: "HabitCategories");

            migrationBuilder.DropIndex(
                name: "IX_Habits_CategoryId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Habits");
        }
    }
}
