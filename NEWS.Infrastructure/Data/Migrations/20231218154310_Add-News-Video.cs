using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWS.Data.Migrations
{
    public partial class AddNewsVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8881f953-e7cc-4d0d-8937-9a74413e60c5",
                column: "ConcurrencyStamp",
                value: "18531690-983a-43b4-9ccd-467e18e8ce19");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcc9e639-b998-466b-8d67-5e7dda1dfe5a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c40c42e5-7314-415b-8d5b-7733023890e8", "AQAAAAEAACcQAAAAENkKY7mXuAFOPbVAFE7vEKjYNmAO/Czp9kaDBlh9E9epDjBZoORsVosmcsfN77/Kmg==", "f1f01567-5b23-488c-8f16-6eef267b1be3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video",
                table: "News");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8881f953-e7cc-4d0d-8937-9a74413e60c5",
                column: "ConcurrencyStamp",
                value: "edaa4c9f-8eba-4623-8f7c-0b32f7d8f7f3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcc9e639-b998-466b-8d67-5e7dda1dfe5a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "078bf50c-d2ac-43b3-89e4-6833af2cae98", "AQAAAAEAACcQAAAAEFgJT8k6aAhpEzSzJXSnZENfZUHcEQiGTpKXSxbJRvic/r1p9L4UhPrt9jBwYMeBzQ==", "7b464606-ae75-4ed6-b094-6add669a5e05" });
        }
    }
}
