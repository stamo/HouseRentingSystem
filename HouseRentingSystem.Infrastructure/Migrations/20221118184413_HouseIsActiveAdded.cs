using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class HouseIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
            //    columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            //    values: new object[] { "4dbb6f5a-3bb3-4427-97ac-b068236cb93f", "AQAAAAEAACcQAAAAEBEymp+5uJYpTKm/Ou7wiMPgZsNlodKgM9u6xau7IZjGdE+CwcFmeArvuLpQ/vT7qQ==", "dbccc4a6-d410-4c90-9245-2e1b8aee1f97" });

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
            //    columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            //    values: new object[] { "f82cc3bd-7443-4013-8e99-24515b3960cc", "AQAAAAEAACcQAAAAEIh29WZ3gVQz7gqvWLCEO3KUOvbZo1oZOTrP1HVi7HFIvCiOx95S2UOCgiLrv/YZrg==", "910f8b54-90b7-46aa-8aa1-1d6845023a94" });

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55af4095-a78d-49af-8ef2-b8e02c557eef", "AQAAAAEAACcQAAAAEBOb5EHXhfhnaLdkhggYwsn4sqhhC8zKpCE7FpNmNYgdXAD4bSrfQqmUOWT6cV+j2Q==", "f1fb8f7f-295b-452c-a5c3-fb0034ab1225" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1bb66d1-9b34-4ee7-b458-be3955cd90dc", "AQAAAAEAACcQAAAAEHIFhfvc9eum1JQCIvl3kM+pVxs2TbG1aKhf3dPstCphH3AuFKFeqb+jK+g8NIucig==", "6b526030-6cf6-440b-84b4-264a89949291" });
        }
    }
}
