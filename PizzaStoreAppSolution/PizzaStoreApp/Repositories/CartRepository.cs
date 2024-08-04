using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing carts.
    /// </summary>
    public class CartRepository : IRepository<int, Cart>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CartRepository(PizzaAppContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new cart to the repository.
        /// </summary>
        /// <param name="item">The cart to add.</param>
        /// <returns>The added cart.</returns>
        /// <exception cref="CartRepositoryException">Thrown when an error occurs while adding the cart.</exception>
        public async Task<Cart> Add(Cart item)
        {
            try
            {
                _logger.LogInformation("Adding Cart to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cart added to the database successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding Cart to the database");
                throw new CartRepositoryException("Error in adding Cart to the database" + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a cart by its key.
        /// </summary>
        /// <param name="key">The key of the cart to delete.</param>
        /// <returns>The deleted cart.</returns>
        /// <exception cref="CartNotFoundException">Thrown when the cart is not found.</exception>
        /// <exception cref="CartRepositoryException">Thrown when an error occurs while deleting the cart.</exception>
        public async Task<Cart> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting Cart from the database");
                var cart = await GetByKey(key);
                if (cart == null)
                {
                    _logger.LogError("Cart not found in the database");
                    throw new CartNotFoundException("Cart not found");
                }
                _context.Remove(cart);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cart deleted from the database successfully.");
                return cart;
            }
            catch (CartNotFoundException ex)
            {
                _logger.LogError(ex, "Cart not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting Cart from the database");
                throw new CartRepositoryException("Error in deleting Cart from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all carts.
        /// </summary>
        /// <returns>A list of all carts.</returns>
        /// <exception cref="CartRepositoryException">Thrown when an error occurs while getting the carts.</exception>
        public async Task<IEnumerable<Cart>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all Carts from the database");
                var carts = await _context.Carts.ToListAsync();
                if (carts.Count <= 0)
                {
                    _logger.LogWarning("No Carts found in the database.");
                    return null;
                }
                _logger.LogInformation("Carts retrieved successfully.");
                return carts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting all Carts from the database");
                throw new CartRepositoryException("Error in getting all Carts from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a cart by its key.
        /// </summary>
        /// <param name="key">The key of the cart to retrieve.</param>
        /// <returns>The retrieved cart.</returns>
        /// <exception cref="CartRepositoryException">Thrown when an error occurs while getting the cart.</exception>
        public async Task<Cart> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting Cart by key from the database");
                var cart = await _context.Carts
                    .Where(c => c.CartId == key)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Pizza)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Size)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Crust)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Beverage)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.CartItemToppings)
                            .ThenInclude(cit => cit.Topping)
                    .FirstOrDefaultAsync();
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found in the database.");
                }
                else
                {
                    _logger.LogInformation("Cart retrieved successfully.");
                }
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Cart by key from the database");
                throw new CartRepositoryException("Error in getting Cart by key from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a cart.
        /// </summary>
        /// <param name="item">The cart to update.</param>
        /// <returns>The updated cart.</returns>
        /// <exception cref="CartNotFoundException">Thrown when the cart is not found.</exception>
        /// <exception cref="CartRepositoryException">Thrown when an error occurs while updating the cart.</exception>
        public async Task<Cart> Update(Cart item)
        {
            try
            {
                _logger.LogInformation("Updating Cart in the database");
                var cart = await GetByKey(item.CartId);
                if (cart == null)
                {
                    _logger.LogError("Cart not found in the database");
                    throw new CartNotFoundException("Cart not found");
                }
                _context.Entry(cart).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cart updated in the database successfully.");
                return item;
            }
            catch (CartNotFoundException ex)
            {
                _logger.LogError(ex, "Cart not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating Cart in the database");
                throw new CartRepositoryException("Error in updating Cart in the database" + ex.Message, ex);
            }
        }

        #endregion
    }
}
