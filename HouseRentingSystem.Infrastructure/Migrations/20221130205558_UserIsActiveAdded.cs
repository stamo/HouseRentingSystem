using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class UserIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a54082fb-5eb7-491a-8b58-6ea8cbf024d8", "AQAAAAEAACcQAAAAEIbc9YCTOfgHRA3CIYnvBJfQGEvvSyFkm8E67Vskf70unZZn9FBSMfHz8CTvMR2mKQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "51d7e0d8-72ff-4f4f-9bde-5d722b80f9a7", "AQAAAAEAACcQAAAAEG9f5TVUZSbKW4rLLBqxp6pEPuG7S/31ct4107pfKVsClHmps37EH7IY5zfix/G+pg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6d037410-a7ab-476f-8923-bbedec5e701f", "AQAAAAEAACcQAAAAEGBVYJvOLgTUZjL4hQMu5tPtKAFXpONEbn2MFhtRvMv1UpSCcRIzbQIPrG47d8Gu5g==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "11971120-931c-4c87-a245-13646c2bea1d", "AQAAAAEAACcQAAAAEBnYNa9pdNiYjf5xpgDRChri3sTwsiqeodqT+cZ1qwHXn13krDbYzFxUruhmWtjbGw==" });
        }
    }
}
