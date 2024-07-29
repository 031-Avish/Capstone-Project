using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    public class CartItemToppingRepository : IRepository<int, CartItemTopping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CartItemToppingRepository> _logger;

        public CartItemToppingRepository(PizzaAppContext context, ILogger<CartItemToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartItemTopping> Add(CartItemTopping item)
        {
            try
            {
                _logger.LogInformation("Adding CartItemTopping to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding CartItemTopping to the database");
                throw new CartItemToppingRepositoryException("Error in adding CartItemTopping to the database" + ex.Message, ex);
            }
        }

        public async Task<CartItemTopping> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting CartItemTopping from the database");
                var cartItemTopping = await GetByKey(key);
                if (cartItemTopping == null)
                {
                    throw new CartItemToppingNotFoundException("CartItemTopping not found");
                }
                _context.Remove(cartItemTopping);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItemTopping deleted from the database");
                return cartItemTopping;
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError("CartItemTopping not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting CartItemTopping from the database");
                throw new CartItemToppingRepositoryException("Error in deleting CartItemTopping from the database" + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CartItemTopping>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all CartItemToppings from the database");
                var cartItemToppings = await _context.CartItemToppings.ToListAsync();
                if (cartItemToppings.Count <= 0)
                {
                    return null;
                }
                return cartItemToppings;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all CartItemToppings from the database");
                throw new CartItemToppingRepositoryException("Error in getting all CartItemToppings from the database" + ex.Message, ex);
            }
        }

        public async Task<CartItemTopping?> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting CartItemTopping by key from the database");
                var cartItemTopping = await _context.CartItemToppings
                    .Where(cit => cit.CartItemToppingId == key)
                    .FirstOrDefaultAsync();
                return cartItemTopping;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting CartItemTopping by key from the database");
                throw new CartItemToppingRepositoryException("Error in getting CartItemTopping by key from the database" + ex.Message, ex);
            }
        }

        public async Task<CartItemTopping> Update(CartItemTopping item)
        {
            try
            {
                _logger.LogInformation("Updating CartItemTopping in the database");
                var cartItemTopping = await GetByKey(item.CartItemToppingId);
                if (cartItemTopping == null)
                {
                    throw new CartItemToppingNotFoundException("CartItemTopping not found");
                }
                _context.Entry(cartItemTopping).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("CartItemTopping updated in the database");
                return item;
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError("CartItemTopping not found in the database");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating CartItemTopping in the database");
                throw new CartItemToppingRepositoryException("Error in updating CartItemTopping in the database" + ex.Message, ex);
            }
        }
    }
}

