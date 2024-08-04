using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing pizzas.
    /// </summary>
    public class PizzaRepository : IRepository<int, Pizza>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<PizzaRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public PizzaRepository(PizzaAppContext context, ILogger<PizzaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new pizza to the repository.
        /// </summary>
        /// <param name="item">The pizza to add.</param>
        /// <returns>The added pizza.</returns>
        /// <exception cref="DuplicatePizzaException">Thrown when a pizza with the same name already exists.</exception>
        /// <exception cref="PizzaRepositoryException">Thrown when an error occurs while adding the pizza.</exception>
        public async Task<Pizza> Add(Pizza item)
        {
            try
            {
                _logger.LogInformation("Adding pizza...");

                // Check if the pizza already exists
                var existingPizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.Name == item.Name);
                if (existingPizza != null)
                {
                    _logger.LogError("A pizza with the same name already exists.");
                    throw new DuplicatePizzaException("A pizza with the same name already exists.");
                }

                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Pizza added successfully.");
                return item;
            }
            catch (DuplicatePizzaException ex)
            {
                _logger.LogError(ex, "Error occurred while adding pizza: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding pizza.");
                throw new PizzaRepositoryException("Error occurred while adding pizza: " + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a pizza by its key.
        /// </summary>
        /// <param name="key">The key of the pizza to delete.</param>
        /// <returns>The deleted pizza.</returns>
        /// <exception cref="PizzaNotFoundException">Thrown when the pizza is not found.</exception>
        /// <exception cref="PizzaRepositoryException">Thrown when an error occurs while deleting the pizza.</exception>
        public async Task<Pizza> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting pizza...");
                var pizza = await GetByKey(key);
                if (pizza == null)
                {
                    _logger.LogError("Pizza not found.");
                    throw new PizzaNotFoundException("Pizza with given Id is not found.");
                }

                _context.Remove(pizza);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Pizza deleted successfully.");
                return pizza;
            }
            catch (PizzaNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting pizza: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting pizza.");
                throw new PizzaRepositoryException("Error occurred while deleting pizza: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all pizzas from the repository.
        /// </summary>
        /// <returns>A list of all pizzas.</returns>
        /// <exception cref="PizzaRepositoryException">Thrown when an error occurs while getting all pizzas.</exception>
        public async Task<IEnumerable<Pizza>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all pizzas...");
                var pizzas = await _context.Pizzas.ToListAsync();

                if (pizzas.Count <= 0)
                {
                    _logger.LogWarning("No pizzas found.");
                    return Enumerable.Empty<Pizza>();
                }

                _logger.LogInformation("Pizzas retrieved successfully.");
                return pizzas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all pizzas.");
                throw new PizzaRepositoryException("Error occurred while getting all pizzas: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a pizza by its key.
        /// </summary>
        /// <param name="key">The key of the pizza to retrieve.</param>
        /// <returns>The retrieved pizza, or null if not found.</returns>
        /// <exception cref="PizzaRepositoryException">Thrown when an error occurs while getting the pizza.</exception>
        public async Task<Pizza> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting pizza by Id...");
                var pizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.PizzaId == key);

                if (pizza == null)
                {
                    _logger.LogWarning("Pizza not found.");
                    return null;
                }

                _logger.LogInformation("Pizza retrieved successfully.");
                return pizza;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pizza by Id.");
                throw new PizzaRepositoryException("Error occurred while getting pizza by Id: " + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a pizza in the repository.
        /// </summary>
        /// <param name="item">The pizza to update.</param>
        /// <returns>The updated pizza.</returns>
        /// <exception cref="PizzaNotFoundException">Thrown when the pizza is not found.</exception>
        /// <exception cref="PizzaRepositoryException">Thrown when an error occurs while updating the pizza.</exception>
        public async Task<Pizza> Update(Pizza item)
        {
            try
            {
                _logger.LogInformation("Updating pizza...");
                var pizza = await GetByKey(item.PizzaId);
                if (pizza == null)
                {
                    _logger.LogError("Pizza not found.");
                    throw new PizzaNotFoundException("Pizza with given Id is not found.");
                }

                _context.Entry(pizza).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);

                _logger.LogInformation("Pizza updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating pizza.");
                throw new PizzaRepositoryException("Error occurred while updating pizza: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
