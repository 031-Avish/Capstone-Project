using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Repositories;
using PizzaStoreApp.Services;

namespace PizzaStoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<PizzaAppContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, UserDetail>, UserDetailRepository>();
            builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
            builder.Services.AddScoped<IRepository<int, Pizza>, PizzaRepository>();
            builder.Services.AddScoped<IRepository<int, Order>, OrderRepository>();
            builder.Services.AddScoped<IRepository<int, OrderDetail>, OrderDetailRepository>();
            builder.Services.AddScoped<IRepository<int, OrderTopping>, OrderToppingRepository>();
            builder.Services.AddScoped<IRepository<int,Cart>, CartRepository>();
            builder.Services.AddScoped<IRepository<int, CartItem>, CartItemRepository>();
            builder.Services.AddScoped<IRepository<int, CartItemTopping>, CartItemToppingRepository>();
            builder.Services.AddScoped<IRepository<int, Topping>, ToppingRepository>();
            builder.Services.AddScoped<IRepository<int, Size>, SizeRepository>();
            builder.Services.AddScoped<IRepository<int, Crust>, CrustRepository>();
            builder.Services.AddScoped<IRepository<int,Beverage>, BeverageRepository>();


            builder.Services.AddScoped<PizzaAppContext, PizzaAppContext>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPizzaService, PizzaSevice>();
            builder.Services.AddScoped<IToppingService,ToppingService>();
            builder.Services.AddScoped<IBeverageService, BeverageService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
