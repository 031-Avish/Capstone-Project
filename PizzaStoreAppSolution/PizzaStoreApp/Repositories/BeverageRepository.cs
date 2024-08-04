using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using Microsoft.Extensions.Logging;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing beverages.
    /// </summary>
    public class BeverageRepository : IRepository<int, Beverage>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<BeverageRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public BeverageRepository(PizzaAppContext context, ILogger<BeverageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Adds a new beverage to the repository.
        /// </summary>
        /// <param name="item">The beverage to add.</param>
        /// <returns>The added beverage.</returns>
        /// <exception cref="DuplicateBeverageException">Thrown when a beverage with the same name already exists.</exception>
        /// <exception cref="BeverageRepositoryException">Thrown when an error occurs while adding the beverage.</exception>
        #region Add Beverage
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
        #endregion

        /// <summary>
        /// Deletes a beverage by its key.
        /// </summary>
        /// <param name="key">The key of the beverage to delete.</param>
        /// <returns>The deleted beverage.</returns>
        /// <exception cref="BeverageNotFoundException">Thrown when the beverage is not found.</exception>
        /// <exception cref="BeverageRepositoryException">Thrown when an error occurs while deleting the beverage.</exception>
        #region Delete Beverage
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
        #endregion

        /// <summary>
        /// Gets all beverages.
        /// </summary>
        /// <returns>A list of all beverages.</returns>
        /// <exception cref="BeverageRepositoryException">Thrown when an error occurs while getting the beverages.</exception>
        #region Get All Beverages
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
        #endregion

        /// <summary>
        /// Gets a beverage by its key.
        /// </summary>
        /// <param name="key">The key of the beverage to retrieve.</param>
        /// <returns>The retrieved beverage.</returns>
        /// <exception cref="BeverageRepositoryException">Thrown when an error occurs while getting the beverage.</exception>
        #region Get Beverage By Key
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
        #endregion

        /// <summary>
        /// Updates a beverage.
        /// </summary>
        /// <param name="item">The beverage to update.</param>
        /// <returns>The updated beverage.</returns>
        /// <exception cref="BeverageNotFoundException">Thrown when the beverage is not found.</exception>
        /// <exception cref="BeverageRepositoryException">Thrown when an error occurs while updating the beverage.</exception>
        #region Update Beverage
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

        #endregion
    }
}
