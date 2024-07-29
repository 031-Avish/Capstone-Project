using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    public class PizzaRepository : IRepository<int, Pizza>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<PizzaRepository> _logger;

        public PizzaRepository(PizzaAppContext context, ILogger<PizzaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Pizza> Add(Pizza item)
        {
            try
            {
                _logger.LogInformation("Adding pizza...");
                // check pizza is already available or not 
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
            catch(DuplicatePizzaException ex )
            {
                _logger.LogError(ex, "Error occurred while adding pizza: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding pizza.");
                throw new PizzaRepositoryException("Error occurred while adding pizza." + ex.Message , ex);
            }
        }

        public async Task<Pizza> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting pizza...");
                var pizza = await GetByKey(key);
                if (pizza == null)
                {
                    _logger.LogError("Pizza not found.");
                    throw new PizzaNotFoundException("Pizza with given Id is not found .");
                }
                _context.Remove(pizza);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Pizza deleted successfully.");
                return pizza;

            }
            catch(PizzaNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting pizza: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting pizza.");
                throw new PizzaRepositoryException("Error occurred while deleting pizza." + ex.Message , ex);
            }
        }

        public async Task<IEnumerable<Pizza>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all pizzas...");
                var pizzas = await _context.Pizzas.ToListAsync();
                if (pizzas.Count <= 0)
                {
                    _logger.LogWarning("No pizzas found.");
                    return null;
                }
                _logger.LogInformation("Pizzas retrieved successfully.");
                return pizzas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all pizzas.");
                throw new PizzaRepositoryException("Error occurred while getting all pizzas." + ex.Message, ex);
            }
        }

        public async Task<Pizza> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting pizza by Id...");
                var pizza =await _context.Pizzas.FirstOrDefaultAsync(p => p.PizzaId == key);
                if(pizza == null)
                {
                    _logger.LogWarning("Pizza not found.");
                    return null;
                }
                _logger.LogInformation("Pizza retrieved successfully.");
                return pizza;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pizza by Id.");
                throw new PizzaRepositoryException("Error occurred while getting pizza by Id." + ex.Message, ex);
            }
           
        }

        public async Task<Pizza> Update(Pizza item)
        {
            try
            {
                _logger.LogInformation("Updating pizza...");
                var pizza = GetByKey(item.PizzaId);
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating pizza.");
                throw new PizzaRepositoryException("Error occurred while updating pizza." + ex.Message, ex);

            }
        }
    }
}
