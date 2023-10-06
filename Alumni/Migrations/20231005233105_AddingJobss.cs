using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alumni.Migrations
{
    public partial class AddingJobss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f672b35-d8c5-4784-8de4-4b910ce4864b", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a679a3de-421b-4e6b-b561-428377d73a7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b39da94d-f91e-4d7b-910b-6f2fa36b8e83", "Alumni", "ALUMNI" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dbbe1fbf-e210-4194-8608-a3d17f73fde6", "Alumni", "ALUMNI" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bc14a883-61e7-40f4-8ebb-c72d46daa8a2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "56e685ad-a4e1-4aa3-a02a-1587a25894df", "Admin", "ADMIN" });
        }
    }
}
