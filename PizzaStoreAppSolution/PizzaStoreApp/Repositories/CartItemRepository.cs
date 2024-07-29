using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Repositories
{
    public class CartItemRepository : IRepository<int, CartItem>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartItemRepository> _logger;

        public CartItemRepository(PizzaAppContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartItem> Add(CartItem item)
        {
            try
            {
                _logger.LogInformation("Adding CartItem to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding CartItem to the database");
                throw new CartItemRepositoryException("Error in adding CartItem to the database" + ex.Message, ex);
            }
        }

        public async Task<CartItem> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting CartItem from the database");
                var cartItem = await GetByKey(key);
                if (cartItem == null)
                {
                    throw new CartItemNotFoundException("CartItem not found");
                }
                _context.Remove(cartItem);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItem deleted from the database");
                return cartItem;
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError("CartItem not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting CartItem from the database");
                throw new CartItemRepositoryException("Error in deleting CartItem from the database" + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all CartItems from the database");
                var cartItems = await _context.CartItems.ToListAsync();
                if (cartItems.Count <= 0)
                {
                    return null;
                }
                return cartItems;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all CartItems from the database");
                throw new CartItemRepositoryException("Error in getting all CartItems from the database" + ex.Message, ex);
            }
        }

        public async Task<CartItem?> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting CartItem by key from the database");
                var cartItem = await _context.CartItems
                    .Where(ci => ci.CartItemId == key)
                    .Include(ci => ci.CartItemToppings)
                    .FirstOrDefaultAsync();
                return cartItem;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting CartItem by key from the database");
                throw new CartItemRepositoryException("Error in getting CartItem by key from the database" + ex.Message, ex);
            }
        }

        public async Task<CartItem> Update(CartItem item)
        {
            try
            {
                _logger.LogInformation("Updating CartItem in the database");
                var cartItem = await GetByKey(item.CartItemId);
                if (cartItem == null)
                {
                    throw new CartItemNotFoundException("CartItem not found");
                }
                _context.Entry(cartItem).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItem updated in the database");
                return item;
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError("CartItem not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating CartItem in the database");
                throw new CartItemRepositoryException("Error in updating CartItem in the database" + ex.Message, ex);
            }
        }
    }
}
