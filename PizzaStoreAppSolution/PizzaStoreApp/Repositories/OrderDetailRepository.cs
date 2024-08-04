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
    /// Repository for managing order details.
    /// </summary>
    public class OrderDetailRepository : IRepository<int, OrderDetail>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<OrderDetailRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetailRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderDetailRepository(PizzaAppContext context, ILogger<OrderDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new order detail to the repository.
        /// </summary>
        /// <param name="item">The order detail to add.</param>
        /// <returns>The added order detail.</returns>
        /// <exception cref="OrderDetailRepositoryException">Thrown when an error occurs while adding the order detail.</exception>
        public async Task<OrderDetail> Add(OrderDetail item)
        {
            try
            {
                _logger.LogInformation("Adding OrderDetail to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding OrderDetail to the database: {Message}", ex.Message);
                throw new OrderDetailRepositoryException("Error in adding OrderDetail to the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes an order detail by its key.
        /// </summary>
        /// <param name="key">The key of the order detail to delete.</param>
        /// <returns>The deleted order detail.</returns>
        /// <exception cref="OrderDetailNotFoundException">Thrown when the order detail is not found.</exception>
        /// <exception cref="OrderDetailRepositoryException">Thrown when an error occurs while deleting the order detail.</exception>
        public async Task<OrderDetail> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting OrderDetail from the database");
                var orderDetail = await GetByKey(key);
                if (orderDetail == null)
                {
                    throw new OrderDetailNotFoundException("OrderDetail not found");
                }
                _context.Remove(orderDetail);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("OrderDetail deleted from the database");
                return orderDetail;
            }
            catch (OrderDetailNotFoundException ex)
            {
                _logger.LogError("OrderDetail not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting OrderDetail from the database: {Message}", ex.Message);
                throw new OrderDetailRepositoryException("Error in deleting OrderDetail from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all order details.
        /// </summary>
        /// <returns>A list of all order details.</returns>
        /// <exception cref="OrderDetailNotFoundException">Thrown when no order details are found.</exception>
        /// <exception cref="OrderDetailRepositoryException">Thrown when an error occurs while getting all order details.</exception>
        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all OrderDetails from the database");

                var orderDetails = await _context.OrderDetails
                    .Include(od => od.Pizza)
                    .Include(od => od.Crust)
                    .Include(od => od.Size)
                    .Include(od => od.Beverage)
                    .Include(od => od.OrderToppings)
                        .ThenInclude(ot => ot.Topping)
                    .ToListAsync();

                if (!orderDetails.Any())
                {
                    _logger.LogWarning("No OrderDetails found in the database");
                    throw new OrderDetailNotFoundException("No OrderDetails found");
                }

                _logger.LogInformation("Retrieved all OrderDetails successfully");
                return orderDetails;
            }
            catch (OrderDetailNotFoundException ex)
            {
                _logger.LogError("No OrderDetails found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all OrderDetails from the database: {Message}", ex.Message);
                throw new OrderDetailRepositoryException("Error in getting all OrderDetails from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets an order detail by its key.
        /// </summary>
        /// <param name="key">The key of the order detail to retrieve.</param>
        /// <returns>The retrieved order detail.</returns>
        /// <exception cref="OrderDetailNotFoundException">Thrown when the order detail is not found.</exception>
        /// <exception cref="OrderDetailRepositoryException">Thrown when an error occurs while getting the order detail.</exception>
        public async Task<OrderDetail> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting OrderDetail by key from the database");

                var orderDetail = await _context.OrderDetails
                    .Include(od => od.Pizza)
                    .Include(od => od.Crust)
                    .Include(od => od.Size)
                    .Include(od => od.Beverage)
                    .Include(od => od.OrderToppings)
                        .ThenInclude(ot => ot.Topping)
                    .FirstOrDefaultAsync(od => od.OrderDetailId == key);

                if (orderDetail == null)
                {
                    throw new OrderDetailNotFoundException("OrderDetail not found with given Id");
                }

                _logger.LogInformation("OrderDetail with key {Key} retrieved successfully", key);
                return orderDetail;
            }
            catch (OrderDetailNotFoundException ex)
            {
                _logger.LogError("OrderDetail not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting OrderDetail by key from the database: {Message}", ex.Message);
                throw new OrderDetailRepositoryException("Error in getting OrderDetail by key from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates an order detail.
        /// </summary>
        /// <param name="item">The order detail to update.</param>
        /// <returns>The updated order detail.</returns>
        /// <exception cref="OrderDetailNotFoundException">Thrown when the order detail is not found.</exception>
        /// <exception cref="OrderDetailRepositoryException">Thrown when an error occurs while updating the order detail.</exception>
        public async Task<OrderDetail> Update(OrderDetail item)
        {
            try
            {
                _logger.LogInformation("Updating OrderDetail in the database");

                var orderDetail = await GetByKey(item.OrderDetailId);
                if (orderDetail == null)
                {
                    throw new OrderDetailNotFoundException("OrderDetail not found");
                }

                _context.Entry(orderDetail).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);

                _logger.LogInformation("OrderDetail updated in the database");
                return item;
            }
            catch (OrderDetailNotFoundException ex)
            {
                _logger.LogError("OrderDetail not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating OrderDetail in the database: {Message}", ex.Message);
                throw new OrderDetailRepositoryException("Error in updating OrderDetail in the database: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
