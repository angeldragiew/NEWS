using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWS.Data.Migrations
{
    public partial class AddAmdinUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8881f953-e7cc-4d0d-8937-9a74413e60c5", "edaa4c9f-8eba-4623-8f7c-0b32f7d8f7f3", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bcc9e639-b998-466b-8d67-5e7dda1dfe5a", 0, null, "078bf50c-d2ac-43b3-89e4-6833af2cae98", "myadmin@gmail.com", false, "Admin", "Adminov", false, null, "MYADMIN@GMAIL.COM", "MYADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEFgJT8k6aAhpEzSzJXSnZENfZUHcEQiGTpKXSxbJRvic/r1p9L4UhPrt9jBwYMeBzQ==", null, false, "7b464606-ae75-4ed6-b094-6add669a5e05", false, "myadmin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8881f953-e7cc-4d0d-8937-9a74413e60c5", "bcc9e639-b998-466b-8d67-5e7dda1dfe5a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8881f953-e7cc-4d0d-8937-9a74413e60c5", "bcc9e639-b998-466b-8d67-5e7dda1dfe5a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8881f953-e7cc-4d0d-8937-9a74413e60c5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcc9e639-b998-466b-8d67-5e7dda1dfe5a");
        }
    }
}
