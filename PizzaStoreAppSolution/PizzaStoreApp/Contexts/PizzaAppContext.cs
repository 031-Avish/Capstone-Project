using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Models;

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
            modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Topping>().Property(t => t.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Cart>().Property(c=>c.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<CartItem>().Property(ci => ci.DiscountPercent).HasPrecision(18, 2);

            // Seed data
            modelBuilder.Entity<Pizza>().HasData(
                new Pizza
                {
                    PizzaId = 1,
                    Name = "Margherita",
                    Description = "Classic delight with 100% real mozzarella cheese",
                    BasePrice = 200.00m,
                    IsAvailable = true,
                    IsVegetarian = true,
                    ImageUrl = "margherita.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pizza
                {
                    PizzaId = 2,
                    Name = "Pepperoni",
                    Description = "A classic American taste! Relish the delectable flavor of Chicken Pepperoni, topped with extra cheese",
                    BasePrice = 220.00m,
                    IsAvailable = true,
                    IsVegetarian = false,
                    ImageUrl = "pepperoni.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pizza
                {
                    PizzaId = 3,
                    Name = "Veggie Supreme",
                    Description = "Loaded with crunchy onions, crisp capsicum, juicy tomatoes and jalapeno with extra cheese",
                    BasePrice = 260.00m,
                    IsAvailable = true,
                    IsVegetarian = true,
                    ImageUrl = "veggie_supreme.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );

            modelBuilder.Entity<Size>().HasData(
                new Size
                {
                    SizeId = 1,
                    SizeName = "Small",
                    SizeMultiplier = 1.0m
                },
                new Size
                {
                    SizeId = 2,
                    SizeName = "Medium",
                    SizeMultiplier = 1.5m
                },
                new Size
                {
                    SizeId = 3,
                    SizeName = "Large",
                    SizeMultiplier = 2.0m
                }
            );

            modelBuilder.Entity<Crust>().HasData(
                new Crust
                {
                    CrustId = 1,
                    CrustName = "Hand Tossed",
                    PriceMultiplier = 1.0m
                },
                new Crust
                {
                    CrustId = 2,
                    CrustName = "Cheese Burst",
                    PriceMultiplier = 1.2m
                },
                new Crust
                {
                    CrustId = 3,
                    CrustName = "Wheat Thin Crust",
                    PriceMultiplier = 1.1m
                },
                new Crust
                {
                    CrustId = 4,
                    CrustName = "New Hand Tossed",
                    PriceMultiplier = 1.0m
                },
                new Crust
                {
                    CrustId = 5,
                    CrustName = "Fresh Pan Pizza",
                    PriceMultiplier = 1.5m
                }
            );

            modelBuilder.Entity<Topping>().HasData(
                new Topping
                {
                    ToppingId = 1,
                    ToppingName = "Onion",
                    IsVegetarian = true,
                    Price = 60.0m,
                    Image = "onion.jpg"
                },
                new Topping
                {
                    ToppingId = 2,
                    ToppingName = "Capsicum",
                    IsVegetarian = true,
                    Price = 40.0m,
                    Image = "capsicum.jpg"
                },
                new Topping
                {
                    ToppingId = 3,
                    ToppingName = "Mushroom",
                    IsVegetarian = true,
                    Price = 60.0m,
                    Image = "mushroom.jpg"
                },
                new Topping
                {
                    ToppingId = 4,
                    ToppingName = "Chicken Sausage",
                    IsVegetarian = false,
                    Price = 70.0m,
                    Image = "chicken_sausage.jpg"
                },
                new Topping
                {
                    ToppingId = 5,
                    ToppingName = "Pepperoni",
                    IsVegetarian = false,
                    Price = 80.0m,
                    Image = "pepperoni.jpg"
                }
            );

            modelBuilder.Entity<Beverage>().HasData(
                new Beverage
                {
                    BeverageId = 1,
                    Name = "Coca Cola",
                    Price = 40.0m,
                    Image = "coca_cola.jpg"
                },
                new Beverage
                {
                    BeverageId = 2,
                    Name = "Pepsi",
                    Price = 45.0m,
                    Image = "pepsi.jpg"
                },
                new Beverage
                {
                    BeverageId = 3,
                    Name = "Sprite",
                    Price = 45.0m,
                    Image = "sprite.jpg"
                },
                new Beverage
                {
                    BeverageId = 4,
                    Name = "Fanta",
                    Price = 40.0m,
                    Image = "fanta.jpg"
                },
                new Beverage
                {
                    BeverageId = 5,
                    Name = "Mountain Dew",
                    Price = 50.0m,
                    Image = "mountain_dew.jpg"
                }
            );
        }
    }
}
