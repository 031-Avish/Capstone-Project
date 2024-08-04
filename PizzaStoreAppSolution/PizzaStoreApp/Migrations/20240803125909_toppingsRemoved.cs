using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStoreApp.Migrations
{
    public partial class toppingsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "ToppingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "ToppingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "ToppingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "ToppingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Toppings",
                keyColumn: "ToppingId",
                keyValue: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "ToppingId", "Image", "IsAvailable", "IsVegetarian", "Price", "ToppingName" },
                values: new object[,]
                {
                    { 1, "onion.jpg", true, true, 60.0m, "Onion" },
                    { 2, "capsicum.jpg", true, true, 40.0m, "Capsicum" },
                    { 3, "mushroom.jpg", true, true, 60.0m, "Mushroom" },
                    { 4, "chicken_sausage.jpg", true, false, 70.0m, "Chicken Sausage" },
                    { 5, "pepperoni.jpg", true, false, 80.0m, "Pepperoni" }
                });
        }
    }
}
