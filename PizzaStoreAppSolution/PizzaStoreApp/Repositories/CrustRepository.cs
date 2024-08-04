using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing crusts.
    /// </summary>
    public class CrustRepository : IRepository<int, Crust>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<CrustRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrustRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CrustRepository(PizzaAppContext context, ILogger<CrustRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new crust to the repository.
        /// </summary>
        /// <param name="item">The crust to add.</param>
        /// <returns>The added crust.</returns>
        /// <exception cref="DuplicateCrustException">Thrown when a crust with the same name already exists.</exception>
        /// <exception cref="CrustRepositoryException">Thrown when an error occurs while adding the crust.</exception>
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

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a crust by its key.
        /// </summary>
        /// <param name="key">The key of the crust to delete.</param>
        /// <returns>The deleted crust.</returns>
        /// <exception cref="CrustNotFoundException">Thrown when the crust is not found.</exception>
        /// <exception cref="CrustRepositoryException">Thrown when an error occurs while deleting the crust.</exception>
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

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all crusts.
        /// </summary>
        /// <returns>A list of all crusts.</returns>
        /// <exception cref="CrustRepositoryException">Thrown when an error occurs while getting the crusts.</exception>
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

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a crust by its key.
        /// </summary>
        /// <param name="key">The key of the crust to retrieve.</param>
        /// <returns>The retrieved crust.</returns>
        /// <exception cref="CrustRepositoryException">Thrown when an error occurs while getting the crust.</exception>
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

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a crust.
        /// </summary>
        /// <param name="item">The crust to update.</param>
        /// <returns>The updated crust.</returns>
        /// <exception cref="CrustNotFoundException">Thrown when the crust is not found.</exception>
        /// <exception cref="CrustRepositoryException">Thrown when an error occurs while updating the crust.</exception>
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

        #endregion
    }
}
