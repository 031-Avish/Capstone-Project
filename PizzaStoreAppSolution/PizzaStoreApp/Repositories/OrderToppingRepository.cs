using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;


namespace PizzaStoreApp.Repositories
{
    public class OrderToppingRepository : IRepository<int, OrderTopping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<OrderToppingRepository> _logger;

        public OrderToppingRepository(PizzaAppContext context, ILogger<OrderToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderTopping> Add(OrderTopping item)
        {
            try
            {
                _logger.LogInformation("Adding OrderTopping to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding OrderTopping to the database: {Message}", ex.Message);
                throw new OrderToppingRepositoryException("Error in adding OrderTopping to the database: " + ex.Message, ex);
            }
        }

        public async Task<OrderTopping> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting OrderTopping from the database");
                var orderTopping = await GetByKey(key);
                if (orderTopping == null)
                {
                    throw new OrderToppingNotFoundException("OrderTopping not found");
                }
                _context.Remove(orderTopping);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("OrderTopping deleted from the database");
                return orderTopping;
            }
            catch (OrderToppingNotFoundException ex)
            {
                _logger.LogError("OrderTopping not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting OrderTopping from the database: {Message}", ex.Message);
                throw new OrderToppingRepositoryException("Error in deleting OrderTopping from the database: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<OrderTopping>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all OrderToppings from the database");

                var orderToppings = await _context.OrderToppings
                    .Include(ot => ot.Topping)
                    .ToListAsync();

                if (!orderToppings.Any())
                {
                    _logger.LogWarning("No OrderToppings found in the database");
                    throw new OrderToppingNotFoundException("No OrderToppings found");
                }

                _logger.LogInformation("Retrieved all OrderToppings successfully");
                return orderToppings;
            }
            catch (OrderToppingNotFoundException ex)
            {
                _logger.LogError("No OrderToppings found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all OrderToppings from the database: {Message}", ex.Message);
                throw new OrderToppingRepositoryException("Error in getting all OrderToppings from the database: " + ex.Message, ex);
            }
        }

        public async Task<OrderTopping> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting OrderTopping by key from the database");

                var orderTopping = await _context.OrderToppings
                    .Include(ot => ot.Topping)
                    .FirstOrDefaultAsync(ot => ot.OrderToppingId == key);

                if (orderTopping == null)
                {
                    throw new OrderToppingNotFoundException("OrderTopping not found with given Id");
                }

                _logger.LogInformation("OrderTopping with key {Key} retrieved successfully", key);
                return orderTopping;
            }
            catch (OrderToppingNotFoundException ex)
            {
                _logger.LogError("OrderTopping not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting OrderTopping by key from the database: {Message}", ex.Message);
                throw new OrderToppingRepositoryException("Error in getting OrderTopping by key from the database: " + ex.Message, ex);
            }
        }

        public async Task<OrderTopping> Update(OrderTopping item)
        {
            try
            {
                _logger.LogInformation("Updating OrderTopping in the database");

                var orderTopping = await GetByKey(item.OrderToppingId);
                if (orderTopping == null)
                {
                    throw new OrderToppingNotFoundException("OrderTopping not found");
                }

                _context.Entry(orderTopping).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);

                _logger.LogInformation("OrderTopping updated in the database");
                return item;
            }
            catch (OrderToppingNotFoundException ex)
            {
                _logger.LogError("OrderTopping not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating OrderTopping in the database: {Message}", ex.Message);
                throw new OrderToppingRepositoryException("Error in updating OrderTopping in the database: " + ex.Message, ex);
            }
        }
    }
}
