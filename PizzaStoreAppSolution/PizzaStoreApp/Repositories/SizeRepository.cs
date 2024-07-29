using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    public class SizeRepository : IRepository<int, Size>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<SizeRepository> _logger;

        public SizeRepository(PizzaAppContext context, ILogger<SizeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Size> Add(Size item)
        {
            try
            {
                _logger.LogInformation("Adding size...");
                var existingSize = await _context.Sizes.FirstOrDefaultAsync(s => s.SizeName == item.SizeName);
                if (existingSize != null)
                {
                    _logger.LogError("A size with the same name already exists.");
                    throw new DuplicateSizeException("A size with the same name already exists.");
                }
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Size added successfully.");
                return item;
            }
            catch (DuplicateSizeException ex)
            {
                _logger.LogError(ex, "Error occurred while adding size: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding size.");
                throw new SizeRepositoryException("Error occurred while adding size." + ex.Message, ex);
            }
        }

        public async Task<Size> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting size...");
                var size = await GetByKey(key);
                if (size == null)
                {
                    _logger.LogError("Size not found.");
                    throw new SizeNotFoundException("Size with given Id is not found.");
                }
                _context.Remove(size);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Size deleted successfully.");
                return size;
            }
            catch (SizeNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting size: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting size.");
                throw new SizeRepositoryException("Error occurred while deleting size." + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Size>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all sizes...");
                var sizes = await _context.Sizes.ToListAsync();
                if (sizes.Count <= 0)
                {
                    _logger.LogWarning("No sizes found.");
                    return null;
                }
                _logger.LogInformation("Sizes retrieved successfully.");
                return sizes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all sizes.");
                throw new SizeRepositoryException("Error occurred while getting all sizes." + ex.Message, ex);
            }
        }

        public async Task<Size> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting size by Id...");
                var size = await _context.Sizes.FirstOrDefaultAsync(s => s.SizeId == key);
                if (size == null)
                {
                    _logger.LogWarning("Size not found.");
                    return null;
                }
                _logger.LogInformation("Size retrieved successfully.");
                return size;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting size by Id.");
                throw new SizeRepositoryException("Error occurred while getting size by Id." + ex.Message, ex);
            }
        }

        public async Task<Size> Update(Size item)
        {
            try
            {
                _logger.LogInformation("Updating size...");
                var size = await GetByKey(item.SizeId);
                if (size == null)
                {
                    _logger.LogError("Size not found.");
                    throw new SizeNotFoundException("Size with given Id is not found.");
                }
                _context.Entry(size).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Size updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating size.");
                throw new SizeRepositoryException("Error occurred while updating size." + ex.Message, ex);
            }
        }
    }
}
