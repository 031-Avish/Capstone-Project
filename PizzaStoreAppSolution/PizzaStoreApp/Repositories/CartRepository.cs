using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Repositories
{
    public class CartRepository:IRepository<int,Cart>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(PizzaAppContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart> Add(Cart item)
        {
            try
            {
                _logger.LogInformation("Adding Cart to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error in adding Cart to the database");
                throw new CartRepositoryException("Error in adding Cart to the database"+ex.Message, ex);
            }
        }

        public async Task<Cart> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting Cart from the database");
                var cart= await GetByKey(key);
                if (cart == null)
                {
                    throw new CartNotFoundException("Cart not found");
                }
                _context.Remove(cart);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cart deleted from the database");
                return cart;
            }
            catch(CartNotFoundException ex)
            {
                _logger.LogError("Cart not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting Cart from the database");
                throw new CartRepositoryException("Error in deleting Cart from the database" + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all Carts from the database");
                var carts = await _context.Carts.ToListAsync();
                if(carts.Count <= 0)
                {
                    return null;
                }
                return carts;

            }
            catch(Exception ex)
            {
                _logger.LogError("Error in getting all Carts from the database");
                throw new CartRepositoryException("Error in getting all Carts from the database" + ex.Message, ex);
            }
        }

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
                        .ThenInclude(ci=> ci.Beverage)
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.CartItemToppings)
                            .ThenInclude(cit => cit.Topping)
                    .FirstOrDefaultAsync();
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting Cart by key from the database");
                throw new CartRepositoryException("Error in getting Cart by key from the database" + ex.Message, ex);
            }
        }

        public async Task<Cart> Update(Cart item)
        {
            try
            {
                _logger.LogInformation("Updating Cart in the database");
                var cart = await GetByKey(item.CartId);
                if (cart == null)
                {
                    throw new CartNotFoundException("Cart not found");
                }
                _context.Entry(cart).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cart updated in the database");
                return item;
            }
            catch (CartNotFoundException ex)
            {
                _logger.LogError("Cart not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating Cart in the database");
                throw new CartRepositoryException("Error in updating Cart in the database" + ex.Message, ex);
            }
        }
    }
}
