using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStoreApp.Migrations
{
    public partial class changes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Beverages",
                keyColumn: "BeverageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Beverages",
                keyColumn: "BeverageId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Beverages",
                keyColumn: "BeverageId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Beverages",
                keyColumn: "BeverageId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Beverages",
                keyColumn: "BeverageId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "PizzaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "PizzaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "PizzaId",
                keyValue: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Beverages",
                columns: new[] { "BeverageId", "Image", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "coca_cola.jpg", true, "Coca Cola", 40.0m },
                    { 2, "pepsi.jpg", true, "Pepsi", 45.0m },
                    { 3, "sprite.jpg", true, "Sprite", 45.0m },
                    { 4, "fanta.jpg", true, "Fanta", 40.0m },
                    { 5, "mountain_dew.jpg", true, "Mountain Dew", 50.0m }
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "PizzaId", "BasePrice", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "IsVegetarian", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 200.00m, new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6135), "Classic delight with 100% real mozzarella cheese", "margherita.jpg", true, true, "Margherita", new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6135) },
                    { 2, 220.00m, new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6138), "A classic American taste! Relish the delectable flavor of Chicken Pepperoni, topped with extra cheese", "pepperoni.jpg", true, false, "Pepperoni", new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6138) },
                    { 3, 260.00m, new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6140), "Loaded with crunchy onions, crisp capsicum, juicy tomatoes and jalapeno with extra cheese", "veggie_supreme.jpg", true, true, "Veggie Supreme", new DateTime(2024, 8, 1, 12, 20, 42, 556, DateTimeKind.Local).AddTicks(6140) }
                });
        }
    }
}
