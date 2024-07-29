using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using System.Runtime.Serialization;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;


namespace PizzaStoreApp.Repositories
{
    public class CrustRepository : IRepository<int, Crust>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CrustRepository> _logger;

        public CrustRepository(PizzaAppContext context, ILogger<CrustRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Crust> Add(Crust item)
        {
            try
            {
                _logger.LogInformation("Adding crust...");
                var existingCrust = await _context.Crusts.FirstOrDefaultAsync(c => c.CrustName == item.CrustName);
                if (existingCrust != null)
                {
                    _logger.LogError("A crust with the same name already exists.");
                    throw new DuplicateCrustException("A crust with the same name already exists.");
                }
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Crust added successfully.");
                return item;
            }
            catch (DuplicateCrustException ex)
            {
                _logger.LogError(ex, "Error occurred while adding crust: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding crust.");
                throw new CrustRepositoryException("Error occurred while adding crust." + ex.Message, ex);
            }
        }

        public async Task<Crust> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting crust...");
                var crust = await GetByKey(key);
                if (crust == null)
                {
                    _logger.LogError("Crust not found.");
                    throw new CrustNotFoundException("Crust with given Id is not found.");
                }
                _context.Remove(crust);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Crust deleted successfully.");
                return crust;
            }
            catch (CrustNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting crust: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting crust.");
                throw new CrustRepositoryException("Error occurred while deleting crust." + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Crust>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all crusts...");
                var crusts = await _context.Crusts.ToListAsync();
                if (crusts.Count <= 0)
                {
                    _logger.LogWarning("No crusts found.");
                    return null;
                }
                _logger.LogInformation("Crusts retrieved successfully.");
                return crusts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all crusts.");
                throw new CrustRepositoryException("Error occurred while getting all crusts." + ex.Message, ex);
            }
        }

        public async Task<Crust> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting crust by Id...");
                var crust = await _context.Crusts.FirstOrDefaultAsync(c => c.CrustId == key);
                if (crust == null)
                {
                    _logger.LogWarning("Crust not found.");
                    return null;
                }
                _logger.LogInformation("Crust retrieved successfully.");
                return crust;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting crust by Id.");
                throw new CrustRepositoryException("Error occurred while getting crust by Id." + ex.Message, ex);
            }
        }

        public async Task<Crust> Update(Crust item)
        {
            try
            {
                _logger.LogInformation("Updating crust...");
                var crust = await GetByKey(item.CrustId);
                if (crust == null)
                {
                    _logger.LogError("Crust not found.");
                    throw new CrustNotFoundException("Crust with given Id is not found.");
                }
                _context.Entry(crust).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Crust updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating crust.");
                throw new CrustRepositoryException("Error occurred while updating crust." + ex.Message, ex);
            }
        }
    }
}
