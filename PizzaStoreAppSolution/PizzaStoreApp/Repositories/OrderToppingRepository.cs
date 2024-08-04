using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing order toppings.
    /// </summary>
    public class OrderToppingRepository : IRepository<int, OrderTopping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<OrderToppingRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderToppingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderToppingRepository(PizzaAppContext context, ILogger<OrderToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new order topping to the repository.
        /// </summary>
        /// <param name="item">The order topping to add.</param>
        /// <returns>The added order topping.</returns>
        /// <exception cref="OrderToppingRepositoryException">Thrown when an error occurs while adding the order topping.</exception>
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

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes an order topping by its key.
        /// </summary>
        /// <param name="key">The key of the order topping to delete.</param>
        /// <returns>The deleted order topping.</returns>
        /// <exception cref="OrderToppingNotFoundException">Thrown when the order topping is not found.</exception>
        /// <exception cref="OrderToppingRepositoryException">Thrown when an error occurs while deleting the order topping.</exception>
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

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all order toppings from the repository.
        /// </summary>
        /// <returns>A list of all order toppings.</returns>
        /// <exception cref="OrderToppingNotFoundException">Thrown when no order toppings are found.</exception>
        /// <exception cref="OrderToppingRepositoryException">Thrown when an error occurs while getting all order toppings.</exception>
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

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets an order topping by its key.
        /// </summary>
        /// <param name="key">The key of the order topping to retrieve.</param>
        /// <returns>The retrieved order topping.</returns>
        /// <exception cref="OrderToppingNotFoundException">Thrown when the order topping is not found.</exception>
        /// <exception cref="OrderToppingRepositoryException">Thrown when an error occurs while getting the order topping.</exception>
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

        #endregion

        #region Update Method

        /// <summary>
        /// Updates an order topping in the repository.
        /// </summary>
        /// <param name="item">The order topping to update.</param>
        /// <returns>The updated order topping.</returns>
        /// <exception cref="OrderToppingNotFoundException">Thrown when the order topping is not found.</exception>
        /// <exception cref="OrderToppingRepositoryException">Thrown when an error occurs while updating the order topping.</exception>
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

        #endregion
    }
}
