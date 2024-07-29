using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    public class ToppingRepository : IRepository<int, Topping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<ToppingRepository> _logger;

        public ToppingRepository(PizzaAppContext context, ILogger<ToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Topping> Add(Topping item)
        {
            try
            {
                _logger.LogInformation("Adding topping...");
                var existingTopping = await _context.Toppings.FirstOrDefaultAsync(t => t.ToppingName == item.ToppingName);
                if (existingTopping != null)
                {
                    _logger.LogError("A topping with the same name already exists.");
                    throw new DuplicateToppingException("A topping with the same name already exists.");
                }
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Topping added successfully.");
                return item;
            }
            catch (DuplicateToppingException ex)
            {
                _logger.LogError(ex, "Error occurred while adding topping: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding topping.");
                throw new ToppingRepositoryException("Error occurred while adding topping." + ex.Message, ex);
            }
        }

        public async Task<Topping> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting topping...");
                var topping = await GetByKey(key);
                if (topping == null)
                {
                    _logger.LogError("Topping not found.");
                    throw new ToppingNotFoundException("Topping with given Id is not found.");
                }
                _context.Remove(topping);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Topping deleted successfully.");
                return topping;
            }
            catch (ToppingNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting topping: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting topping.");
                throw new ToppingRepositoryException("Error occurred while deleting topping." + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Topping>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all toppings...");
                var toppings = await _context.Toppings.ToListAsync();
                if (toppings.Count <= 0)
                {
                    _logger.LogWarning("No toppings found.");
                    return null;
                }
                _logger.LogInformation("Toppings retrieved successfully.");
                return toppings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all toppings.");
                throw new ToppingRepositoryException("Error occurred while getting all toppings." + ex.Message, ex);
            }
        }

        public async Task<Topping> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting topping by Id...");
                var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.ToppingId == key);
                if (topping == null)
                {
                    _logger.LogWarning("Topping not found.");
                    return null;
                }
                _logger.LogInformation("Topping retrieved successfully.");
                return topping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting topping by Id.");
                throw new ToppingRepositoryException("Error occurred while getting topping by Id." + ex.Message, ex);
            }
        }

        public async Task<Topping> Update(Topping item)
        {
            try
            {
                _logger.LogInformation("Updating topping...");
                var topping = await GetByKey(item.ToppingId);
                if (topping == null)
                {
                    _logger.LogError("Topping not found.");
                    throw new ToppingNotFoundException("Topping with given Id is not found.");
                }
                _context.Entry(topping).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Topping updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating topping.");
                throw new ToppingRepositoryException("Error occurred while updating topping." + ex.Message, ex);
            }
        }
    }
}

