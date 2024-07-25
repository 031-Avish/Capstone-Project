using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Models;
using System.Collections.Generic;

namespace PizzaStoreApp.Contexts
{

    public class PizzaAppContext : DbContext
    {
        public PizzaAppContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Crust> Crusts { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderTopping> OrderToppings { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemTopping> CartItemToppings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify precision and scale for decimal properties
            modelBuilder.Entity<Pizza>().Property(p => p.BasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<Size>().Property(s => s.SizeMultiplier).HasPrecision(18, 2);
            modelBuilder.Entity<Crust>().Property(c => c.PriceMultiplier).HasPrecision(18, 2);
            modelBuilder.Entity<Beverage>().Property(b => b.Price).HasPrecision(18, 2);
            modelBuilder.Entity<OrderDetail>().Property(od => od.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<CartItem>().Property(ci => ci.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o=>o.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Topping>().Property(t=>t.Price).HasPrecision(18, 2);
        }
    }
}


