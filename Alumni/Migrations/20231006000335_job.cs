using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alumni.Migrations
{
    public partial class job : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOpportunity_AspNetUsers_UserId",
                table: "JobOpportunity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOpportunity",
                table: "JobOpportunity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameTable(
                name: "JobOpportunity",
                newName: "JobOpportunities");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_JobOpportunity_UserId",
                table: "JobOpportunities",
                newName: "IX_JobOpportunities_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_UserId",
                table: "Admins",
                newName: "IX_Admins_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOpportunities",
                table: "JobOpportunities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0c1fb01c-7ec5-4f0a-9ee9-d5543aeca04e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8b2dd84e-5c03-4fbc-8234-bfc0ccdf583d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3, "b0b0933e-de7e-4a08-9257-b88b749007a2", "admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOpportunities_AspNetUsers_UserId",
                table: "JobOpportunities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOpportunities_AspNetUsers_UserId",
                table: "JobOpportunities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOpportunities",
                table: "JobOpportunities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "JobOpportunities",
                newName: "JobOpportunity");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.RenameIndex(
                name: "IX_JobOpportunities_UserId",
                table: "JobOpportunity",
                newName: "IX_JobOpportunity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_UserId",
                table: "Admin",
                newName: "IX_Admin_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOpportunity",
                table: "JobOpportunity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5b5cd520-4835-446e-b48f-9e1f12a3053e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "50687145-658e-477c-b881-96d8db7df8dc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 4, "f356bf46-c355-45d3-ba1c-848515b18d66", "admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOpportunity_AspNetUsers_UserId",
                table: "JobOpportunity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
