using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    public class BeverageRepository : IRepository<int, Beverage>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<BeverageRepository> _logger;

        public BeverageRepository(PizzaAppContext context, ILogger<BeverageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Beverage> Add(Beverage item)
        {
            try
            {
                _logger.LogInformation("Adding beverage...");
                var existingBeverage = await _context.Beverages.FirstOrDefaultAsync(b => b.Name == item.Name);
                if (existingBeverage != null)
                {
                    _logger.LogError("A beverage with the same name already exists.");
                    throw new DuplicateBeverageException("A beverage with the same name already exists.");
                }
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Beverage added successfully.");
                return item;
            }
            catch (DuplicateBeverageException ex)
            {
                _logger.LogError(ex, "Error occurred while adding beverage: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding beverage.");
                throw new BeverageRepositoryException("Error occurred while adding beverage." + ex.Message, ex);
            }
        }

        public async Task<Beverage> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting beverage...");
                var beverage = await GetByKey(key);
                if (beverage == null)
                {
                    _logger.LogError("Beverage not found.");
                    throw new BeverageNotFoundException("Beverage with given Id is not found.");
                }
                _context.Remove(beverage);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Beverage deleted successfully.");
                return beverage;
            }
            catch (BeverageNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting beverage: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting beverage.");
                throw new BeverageRepositoryException("Error occurred while deleting beverage." + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Beverage>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all beverages...");
                var beverages = await _context.Beverages.ToListAsync();
                if (beverages.Count <= 0)
                {
                    _logger.LogWarning("No beverages found.");
                    return null;
                }
                _logger.LogInformation("Beverages retrieved successfully.");
                return beverages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all beverages.");
                throw new BeverageRepositoryException("Error occurred while getting all beverages." + ex.Message, ex);
            }
        }

        public async Task<Beverage> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting beverage by Id...");
                var beverage = await _context.Beverages.FirstOrDefaultAsync(b => b.BeverageId == key);
                if (beverage == null)
                {
                    _logger.LogWarning("Beverage not found.");
                    return null;
                }
                _logger.LogInformation("Beverage retrieved successfully.");
                return beverage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting beverage by Id.");
                throw new BeverageRepositoryException("Error occurred while getting beverage by Id." + ex.Message, ex);
            }
        }

        public async Task<Beverage> Update(Beverage item)
        {
            try
            {
                _logger.LogInformation("Updating beverage...");
                var beverage = await GetByKey(item.BeverageId);
                if (beverage == null)
                {
                    _logger.LogError("Beverage not found.");
                    throw new BeverageNotFoundException("Beverage with given Id is not found.");
                }
                _context.Entry(beverage).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Beverage updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating beverage.");
                throw new BeverageRepositoryException("Error occurred while updating beverage." + ex.Message, ex);
            }
        }
    }
}
