using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing cart items.
    /// </summary>
    public class CartItemRepository : IRepository<int, CartItem>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartItemRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CartItemRepository(PizzaAppContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new cart item to the repository.
        /// </summary>
        /// <param name="item">The cart item to add.</param>
        /// <returns>The added cart item.</returns>
        /// <exception cref="CartItemRepositoryException">Thrown when an error occurs while adding the cart item.</exception>
        public async Task<CartItem> Add(CartItem item)
        {
            try
            {
                _logger.LogInformation("Adding CartItem to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItem added to the database successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding CartItem to the database");
                throw new CartItemRepositoryException("Error in adding CartItem to the database" + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a cart item by its key.
        /// </summary>
        /// <param name="key">The key of the cart item to delete.</param>
        /// <returns>The deleted cart item.</returns>
        /// <exception cref="CartItemNotFoundException">Thrown when the cart item is not found.</exception>
        /// <exception cref="CartItemRepositoryException">Thrown when an error occurs while deleting the cart item.</exception>
        public async Task<CartItem> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting CartItem from the database");
                var cartItem = await GetByKey(key);
                if (cartItem == null)
                {
                    _logger.LogError("CartItem not found in the database");
                    throw new CartItemNotFoundException("CartItem not found");
                }
                _context.Remove(cartItem);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItem deleted from the database successfully.");
                return cartItem;
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError(ex, "CartItem not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting CartItem from the database");
                throw new CartItemRepositoryException("Error in deleting CartItem from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all cart items.
        /// </summary>
        /// <returns>A list of all cart items.</returns>
        /// <exception cref="CartItemRepositoryException">Thrown when an error occurs while getting the cart items.</exception>
        public async Task<IEnumerable<CartItem>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all CartItems from the database");
                var cartItems = await _context.CartItems.ToListAsync();
                if (cartItems.Count <= 0)
                {
                    _logger.LogWarning("No CartItems found in the database.");
                    return null;
                }
                _logger.LogInformation("CartItems retrieved successfully.");
                return cartItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting all CartItems from the database");
                throw new CartItemRepositoryException("Error in getting all CartItems from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a cart item by its key.
        /// </summary>
        /// <param name="key">The key of the cart item to retrieve.</param>
        /// <returns>The retrieved cart item.</returns>
        /// <exception cref="CartItemRepositoryException">Thrown when an error occurs while getting the cart item.</exception>
        public async Task<CartItem?> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting CartItem by key from the database");
                var cartItem = await _context.CartItems
                    .Where(ci => ci.CartItemId == key)
                    .Include(ci => ci.CartItemToppings)
                    .FirstOrDefaultAsync();
                if (cartItem == null)
                {
                    _logger.LogWarning("CartItem not found in the database.");
                }
                else
                {
                    _logger.LogInformation("CartItem retrieved successfully.");
                }
                return cartItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting CartItem by key from the database");
                throw new CartItemRepositoryException("Error in getting CartItem by key from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a cart item.
        /// </summary>
        /// <param name="item">The cart item to update.</param>
        /// <returns>The updated cart item.</returns>
        /// <exception cref="CartItemNotFoundException">Thrown when the cart item is not found.</exception>
        /// <exception cref="CartItemRepositoryException">Thrown when an error occurs while updating the cart item.</exception>
        public async Task<CartItem> Update(CartItem item)
        {
            try
            {
                _logger.LogInformation("Updating CartItem in the database");
                var cartItem = await GetByKey(item.CartItemId);
                if (cartItem == null)
                {
                    _logger.LogError("CartItem not found in the database");
                    throw new CartItemNotFoundException("CartItem not found");
                }
                _context.Entry(cartItem).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItem updated in the database successfully.");
                return item;
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError(ex, "CartItem not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating CartItem in the database");
                throw new CartItemRepositoryException("Error in updating CartItem in the database" + ex.Message, ex);
            }
        }

        #endregion
    }
}
