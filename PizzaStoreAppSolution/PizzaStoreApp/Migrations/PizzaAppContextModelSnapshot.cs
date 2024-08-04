﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PizzaStoreApp.Contexts;

#nullable disable

namespace PizzaStoreApp.Migrations
{
    [DbContext(typeof(PizzaAppContext))]
    partial class PizzaAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PizzaStoreApp.Models.Beverage", b =>
                {
                    b.Property<int>("BeverageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BeverageId"), 1L, 1);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BeverageId");

                    b.ToTable("Beverages");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemId"), 1L, 1);

                    b.Property<int?>("BeverageId")
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int?>("CrustId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscountPercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PizzaId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("SizeId")
                        .HasColumnType("int");

                    b.Property<decimal>("SubTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CartItemId");

                    b.HasIndex("BeverageId");

                    b.HasIndex("CartId");

                    b.HasIndex("CrustId");

                    b.HasIndex("PizzaId");

                    b.HasIndex("SizeId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.CartItemTopping", b =>
                {
                    b.Property<int>("CartItemToppingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemToppingId"), 1L, 1);

                    b.Property<int>("CartItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ToppingId")
                        .HasColumnType("int");

                    b.HasKey("CartItemToppingId");

                    b.HasIndex("CartItemId");

                    b.HasIndex("ToppingId");

                    b.ToTable("CartItemToppings");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Crust", b =>
                {
                    b.Property<int>("CrustId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CrustId"), 1L, 1);

                    b.Property<string>("CrustName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("PriceMultiplier")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CrustId");

                    b.ToTable("Crusts");

                    b.HasData(
                        new
                        {
                            CrustId = 1,
                            CrustName = "Hand Tossed",
                            PriceMultiplier = 1.0m
                        },
                        new
                        {
                            CrustId = 2,
                            CrustName = "Cheese Burst",
                            PriceMultiplier = 1.2m
                        },
                        new
                        {
                            CrustId = 3,
                            CrustName = "Wheat Thin Crust",
                            PriceMultiplier = 1.1m
                        },
                        new
                        {
                            CrustId = 4,
                            CrustName = "New Hand Tossed",
                            PriceMultiplier = 1.0m
                        },
                        new
                        {
                            CrustId = 5,
                            CrustName = "Fresh Pan Pizza",
                            PriceMultiplier = 1.5m
                        });
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<string>("DeliveryAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPickup")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"), 1L, 1);

                    b.Property<int?>("BeverageId")
                        .HasColumnType("int");

                    b.Property<int?>("CrustId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscountPercent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("PizzaId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("SizeId")
                        .HasColumnType("int");

                    b.Property<decimal>("SubTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("BeverageId");

                    b.HasIndex("CrustId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PizzaId");

                    b.HasIndex("SizeId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.OrderTopping", b =>
                {
                    b.Property<int>("OrderToppingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderToppingId"), 1L, 1);

                    b.Property<int>("OrderDetailId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ToppingId")
                        .HasColumnType("int");

                    b.HasKey("OrderToppingId");

                    b.HasIndex("OrderDetailId");

                    b.HasIndex("ToppingId");

                    b.ToTable("OrderToppings");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("RazorpayOrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RazorpayPaymentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RazorpaySignature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Pizza", b =>
                {
                    b.Property<int>("PizzaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PizzaId"), 1L, 1);

                    b.Property<decimal>("BasePrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PizzaId");

                    b.ToTable("Pizzas");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"), 1L, 1);

                    b.Property<decimal>("SizeMultiplier")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SizeName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            SizeId = 1,
                            SizeMultiplier = 1.0m,
                            SizeName = "Small"
                        },
                        new
                        {
                            SizeId = 2,
                            SizeMultiplier = 1.5m,
                            SizeName = "Medium"
                        },
                        new
                        {
                            SizeId = 3,
                            SizeMultiplier = 2.0m,
                            SizeName = "Large"
                        });
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Topping", b =>
                {
                    b.Property<int>("ToppingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ToppingId"), 1L, 1);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ToppingName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ToppingId");

                    b.ToTable("Toppings");

                    b.HasData(
                        new
                        {
                            ToppingId = 1,
                            Image = "onion.jpg",
                            IsAvailable = true,
                            IsVegetarian = true,
                            Price = 60.0m,
                            ToppingName = "Onion"
                        },
                        new
                        {
                            ToppingId = 2,
                            Image = "capsicum.jpg",
                            IsAvailable = true,
                            IsVegetarian = true,
                            Price = 40.0m,
                            ToppingName = "Capsicum"
                        },
                        new
                        {
                            ToppingId = 3,
                            Image = "mushroom.jpg",
                            IsAvailable = true,
                            IsVegetarian = true,
                            Price = 60.0m,
                            ToppingName = "Mushroom"
                        },
                        new
                        {
                            ToppingId = 4,
                            Image = "chicken_sausage.jpg",
                            IsAvailable = true,
                            IsVegetarian = false,
                            Price = 70.0m,
                            ToppingName = "Chicken Sausage"
                        },
                        new
                        {
                            ToppingId = 5,
                            Image = "pepperoni.jpg",
                            IsAvailable = true,
                            IsVegetarian = false,
                            Price = 80.0m,
                            ToppingName = "Pepperoni"
                        });
                });

            modelBuilder.Entity("PizzaStoreApp.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.UserDetail", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserDetails");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Cart", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.UserDetail", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.CartItem", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.Beverage", "Beverage")
                        .WithMany()
                        .HasForeignKey("BeverageId");

                    b.HasOne("PizzaStoreApp.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PizzaStoreApp.Models.Crust", "Crust")
                        .WithMany()
                        .HasForeignKey("CrustId");

                    b.HasOne("PizzaStoreApp.Models.Pizza", "Pizza")
                        .WithMany()
                        .HasForeignKey("PizzaId");

                    b.HasOne("PizzaStoreApp.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId");

                    b.Navigation("Beverage");

                    b.Navigation("Cart");

                    b.Navigation("Crust");

                    b.Navigation("Pizza");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.CartItemTopping", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.CartItem", "CartItem")
                        .WithMany("CartItemToppings")
                        .HasForeignKey("CartItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PizzaStoreApp.Models.Topping", "Topping")
                        .WithMany()
                        .HasForeignKey("ToppingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartItem");

                    b.Navigation("Topping");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Order", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.UserDetail", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.OrderDetail", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.Beverage", "Beverage")
                        .WithMany()
                        .HasForeignKey("BeverageId");

                    b.HasOne("PizzaStoreApp.Models.Crust", "Crust")
                        .WithMany()
                        .HasForeignKey("CrustId");

                    b.HasOne("PizzaStoreApp.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PizzaStoreApp.Models.Pizza", "Pizza")
                        .WithMany()
                        .HasForeignKey("PizzaId");

                    b.HasOne("PizzaStoreApp.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId");

                    b.Navigation("Beverage");

                    b.Navigation("Crust");

                    b.Navigation("Order");

                    b.Navigation("Pizza");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.OrderTopping", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.OrderDetail", "OrderDetail")
                        .WithMany("OrderToppings")
                        .HasForeignKey("OrderDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PizzaStoreApp.Models.Topping", "Topping")
                        .WithMany()
                        .HasForeignKey("ToppingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderDetail");

                    b.Navigation("Topping");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Payment", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.UserDetail", b =>
                {
                    b.HasOne("PizzaStoreApp.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.CartItem", b =>
                {
                    b.Navigation("CartItemToppings");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("PizzaStoreApp.Models.OrderDetail", b =>
                {
                    b.Navigation("OrderToppings");
                });
#pragma warning restore 612, 618
        }
    }
}
