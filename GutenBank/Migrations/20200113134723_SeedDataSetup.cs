using Microsoft.EntityFrameworkCore.Migrations;

namespace GutenBank.Migrations
{
    public partial class SeedDataSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountNumber", "Balance", "Currency" },
                values: new object[] { 1, 50m, 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountNumber", "Balance", "Currency" },
                values: new object[] { 2, 100m, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountNumber",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountNumber",
                keyValue: 2);
        }
    }
}
