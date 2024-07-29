using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaStoreApp.Repositories
{
    public class OrderRepository : IRepository<int, Order>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(PizzaAppContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> Add(Order item)
        {
            try
            {
                _logger.LogInformation("Adding Order to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding Order to the database: {Message}", ex.Message);
                throw new OrderRepositoryException("Error in adding Order to the database: " + ex.Message, ex);
            }
        }

        public async Task<Order> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting Order from the database");
                var order = await GetByKey(key);
                if (order == null)
                {
                    throw new OrderNotFoundException("Order not found");
                }
                _context.Remove(order);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Order deleted from the database");
                return order;
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogError("Order not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting Order from the database: {Message}", ex.Message);
                throw new OrderRepositoryException("Error in deleting Order from the database: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all Orders from the database");

                var orders = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Pizza)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Crust)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Size)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Beverage)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.OrderToppings)
                            .ThenInclude(ot => ot.Topping)
                    .ToListAsync();

                if (!orders.Any())
                {
                    _logger.LogWarning("No Orders found in the database");
                    throw new OrderNotFoundException("No Orders found");
                }

                _logger.LogInformation("Retrieved all Orders successfully");
                return orders;
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogError("No Orders found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Orders from the database: {Message}", ex.Message);
                throw new OrderRepositoryException("Error in getting all Orders from the database: " + ex.Message, ex);
            }
        }

        public async Task<Order> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting Order by key from the database");

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Pizza)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Crust)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Size)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Beverage)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.OrderToppings)
                            .ThenInclude(ot => ot.Topping)
                    .FirstOrDefaultAsync(o => o.OrderId == key);

                if (order == null)
                {
                    throw new OrderNotFoundException("Order not found with given Id");
                }

                _logger.LogInformation("Order with key {Key} retrieved successfully", key);
                return order;
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogError("Order not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting Order by key from the database: {Message}", ex.Message);
                throw new OrderRepositoryException("Error in getting Order by key from the database: " + ex.Message, ex);
            }
        }

        public async Task<Order> Update(Order item)
        {
            try
            {
                _logger.LogInformation("Updating Order in the database");

                var order = await GetByKey(item.OrderId);
                if (order == null)
                {
                    throw new OrderNotFoundException("Order not found");
                }

                _context.Entry(order).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);

                _logger.LogInformation("Order updated in the database");
                return item;
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogError("Order not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating Order in the database: {Message}", ex.Message);
                throw new OrderRepositoryException("Error in updating Order in the database: " + ex.Message, ex);
            }
        }
    }
}
