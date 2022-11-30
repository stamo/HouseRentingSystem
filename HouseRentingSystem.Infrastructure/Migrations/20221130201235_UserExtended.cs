using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class UserExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d037410-a7ab-476f-8923-bbedec5e701f", "AQAAAAEAACcQAAAAEGBVYJvOLgTUZjL4hQMu5tPtKAFXpONEbn2MFhtRvMv1UpSCcRIzbQIPrG47d8Gu5g==", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "11971120-931c-4c87-a245-13646c2bea1d", "AQAAAAEAACcQAAAAEBnYNa9pdNiYjf5xpgDRChri3sTwsiqeodqT+cZ1qwHXn13krDbYzFxUruhmWtjbGw==", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4dbb6f5a-3bb3-4427-97ac-b068236cb93f", "AQAAAAEAACcQAAAAEBEymp+5uJYpTKm/Ou7wiMPgZsNlodKgM9u6xau7IZjGdE+CwcFmeArvuLpQ/vT7qQ==", "dbccc4a6-d410-4c90-9245-2e1b8aee1f97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f82cc3bd-7443-4013-8e99-24515b3960cc", "AQAAAAEAACcQAAAAEIh29WZ3gVQz7gqvWLCEO3KUOvbZo1oZOTrP1HVi7HFIvCiOx95S2UOCgiLrv/YZrg==", "910f8b54-90b7-46aa-8aa1-1d6845023a94" });
        }
    }
}
