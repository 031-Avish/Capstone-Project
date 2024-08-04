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
    /// <summary>
    /// Repository for managing toppings.
    /// </summary>
    public class ToppingRepository : IRepository<int, Topping>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<ToppingRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToppingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ToppingRepository(PizzaAppContext context, ILogger<ToppingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new topping to the repository.
        /// </summary>
        /// <param name="item">The topping to add.</param>
        /// <returns>The added topping.</returns>
        /// <exception cref="DuplicateToppingException">Thrown when a topping with the same name already exists.</exception>
        /// <exception cref="ToppingRepositoryException">Thrown when an error occurs while adding the topping.</exception>
        public async Task<Topping> Add(Topping item)
        {
            try
            {
                _logger.LogInformation("Adding topping...");

                // Check if the topping already exists
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
                throw new ToppingRepositoryException("Error occurred while adding topping: " + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a topping by its key.
        /// </summary>
        /// <param name="key">The key of the topping to delete.</param>
        /// <returns>The deleted topping.</returns>
        /// <exception cref="ToppingNotFoundException">Thrown when the topping is not found.</exception>
        /// <exception cref="ToppingRepositoryException">Thrown when an error occurs while deleting the topping.</exception>
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
                throw new ToppingRepositoryException("Error occurred while deleting topping: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all toppings from the repository.
        /// </summary>
        /// <returns>A list of all toppings.</returns>
        /// <exception cref="ToppingRepositoryException">Thrown when an error occurs while getting all toppings.</exception>
        public async Task<IEnumerable<Topping>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all toppings...");
                var toppings = await _context.Toppings.ToListAsync();

                if (toppings.Count <= 0)
                {
                    _logger.LogWarning("No toppings found.");
                    return Enumerable.Empty<Topping>();
                }

                _logger.LogInformation("Toppings retrieved successfully.");
                return toppings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all toppings.");
                throw new ToppingRepositoryException("Error occurred while getting all toppings: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a topping by its key.
        /// </summary>
        /// <param name="key">The key of the topping to retrieve.</param>
        /// <returns>The retrieved topping, or null if not found.</returns>
        /// <exception cref="ToppingRepositoryException">Thrown when an error occurs while getting the topping.</exception>
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
                throw new ToppingRepositoryException("Error occurred while getting topping by Id: " + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a topping in the repository.
        /// </summary>
        /// <param name="item">The topping to update.</param>
        /// <returns>The updated topping.</returns>
        /// <exception cref="ToppingNotFoundException">Thrown when the topping is not found.</exception>
        /// <exception cref="ToppingRepositoryException">Thrown when an error occurs while updating the topping.</exception>
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
                throw new ToppingRepositoryException("Error occurred while updating topping: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
