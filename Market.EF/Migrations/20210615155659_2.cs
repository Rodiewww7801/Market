using Microsoft.EntityFrameworkCore.Migrations;

namespace Market.EF.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Info", "Name", "Price", "QuantityInStock" },
                values: new object[] { 1, "T-SHIRT", "100% Cotton\nIf u want one. Add in description \"I want one pls\"", "TEETH PROBLEMS", 65m, 20 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Info", "Name", "Price", "QuantityInStock" },
                values: new object[] { 2, "T-SHIRT", "100% Cotton\nthoughts in my head", "HEAD PROBLEMS", 65m, 20 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Info", "Name", "Price", "QuantityInStock" },
                values: new object[] { 3, "KNIT SWEATER", "2 SIDED SWEATER WITH FULL JACQUARD\nSWEATER MADE FROM EXPENSIVE MERINOS YARN\nOVERSIZED BAGGY FIT\nWarm.Good for winter", "THE BJORK", 145m, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
