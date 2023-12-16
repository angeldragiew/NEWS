using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWS.Data.Migrations
{
    public partial class CategoryLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Categories",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Categories");
        }
    }
}
