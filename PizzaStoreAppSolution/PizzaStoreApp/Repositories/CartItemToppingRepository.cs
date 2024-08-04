using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing cart item toppings.
    /// </summary>
    public class CartItemToppingRepository : IRepository<int, CartItemTopping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartItemToppingRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemToppingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CartItemToppingRepository(PizzaAppContext context, ILogger<CartItemToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new cart item topping to the repository.
        /// </summary>
        /// <param name="item">The cart item topping to add.</param>
        /// <returns>The added cart item topping.</returns>
        /// <exception cref="CartItemToppingRepositoryException">Thrown when an error occurs while adding the cart item topping.</exception>
        public async Task<CartItemTopping> Add(CartItemTopping item)
        {
            try
            {
                _logger.LogInformation("Adding CartItemTopping to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItemTopping added to the database successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding CartItemTopping to the database");
                throw new CartItemToppingRepositoryException("Error in adding CartItemTopping to the database" + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a cart item topping by its key.
        /// </summary>
        /// <param name="key">The key of the cart item topping to delete.</param>
        /// <returns>The deleted cart item topping.</returns>
        /// <exception cref="CartItemToppingNotFoundException">Thrown when the cart item topping is not found.</exception>
        /// <exception cref="CartItemToppingRepositoryException">Thrown when an error occurs while deleting the cart item topping.</exception>
        public async Task<CartItemTopping> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting CartItemTopping from the database");
                var cartItemTopping = await GetByKey(key);
                if (cartItemTopping == null)
                {
                    _logger.LogError("CartItemTopping not found in the database");
                    throw new CartItemToppingNotFoundException("CartItemTopping not found");
                }
                _context.Remove(cartItemTopping);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItemTopping deleted from the database successfully.");
                return cartItemTopping;
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError(ex, "CartItemTopping not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting CartItemTopping from the database");
                throw new CartItemToppingRepositoryException("Error in deleting CartItemTopping from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all cart item toppings.
        /// </summary>
        /// <returns>A list of all cart item toppings.</returns>
        /// <exception cref="CartItemToppingRepositoryException">Thrown when an error occurs while getting the cart item toppings.</exception>
        public async Task<IEnumerable<CartItemTopping>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all CartItemToppings from the database");
                var cartItemToppings = await _context.CartItemToppings.ToListAsync();
                if (cartItemToppings.Count <= 0)
                {
                    _logger.LogWarning("No CartItemToppings found in the database.");
                    return null;
                }
                _logger.LogInformation("CartItemToppings retrieved successfully.");
                return cartItemToppings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting all CartItemToppings from the database");
                throw new CartItemToppingRepositoryException("Error in getting all CartItemToppings from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a cart item topping by its key.
        /// </summary>
        /// <param name="key">The key of the cart item topping to retrieve.</param>
        /// <returns>The retrieved cart item topping.</returns>
        /// <exception cref="CartItemToppingRepositoryException">Thrown when an error occurs while getting the cart item topping.</exception>
        public async Task<CartItemTopping?> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting CartItemTopping by key from the database");
                var cartItemTopping = await _context.CartItemToppings
                    .Where(cit => cit.CartItemToppingId == key)
                    .FirstOrDefaultAsync();
                if (cartItemTopping == null)
                {
                    _logger.LogWarning("CartItemTopping not found in the database.");
                }
                else
                {
                    _logger.LogInformation("CartItemTopping retrieved successfully.");
                }
                return cartItemTopping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting CartItemTopping by key from the database");
                throw new CartItemToppingRepositoryException("Error in getting CartItemTopping by key from the database" + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a cart item topping.
        /// </summary>
        /// <param name="item">The cart item topping to update.</param>
        /// <returns>The updated cart item topping.</returns>
        /// <exception cref="CartItemToppingNotFoundException">Thrown when the cart item topping is not found.</exception>
        /// <exception cref="CartItemToppingRepositoryException">Thrown when an error occurs while updating the cart item topping.</exception>
        public async Task<CartItemTopping> Update(CartItemTopping item)
        {
            try
            {
                _logger.LogInformation("Updating CartItemTopping in the database");
                var cartItemTopping = await GetByKey(item.CartItemToppingId);
                if (cartItemTopping == null)
                {
                    _logger.LogError("CartItemTopping not found in the database");
                    throw new CartItemToppingNotFoundException("CartItemTopping not found");
                }
                _context.Entry(cartItemTopping).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItemTopping updated in the database successfully.");
                return item;
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError(ex, "CartItemTopping not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating CartItemTopping in the database");
                throw new CartItemToppingRepositoryException("Error in updating CartItemTopping in the database" + ex.Message, ex);
            }
        }

        #endregion
    }
}
