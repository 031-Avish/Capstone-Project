using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(PizzaAppContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Add(User item)
        {
            try
            {
                _logger.LogInformation("Adding user...");

                var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == item.Email);
                if (existingUserByEmail != null)
                {
                    _logger.LogError("A user with the same email already exists.");
                    throw new DuplicateUserException("A user with the same email already exists.");
                }

                var existingUserByPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.Phone == item.Phone);
                if (existingUserByPhoneNumber != null)
                {
                    _logger.LogError("A user with the same phone number already exists.");
                    throw new DuplicateUserException("A user with the same phone number already exists.");
                }

                _context.Add(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User added successfully.");
                return item;
            }
            catch (DuplicateUserException ex)
            {
                _logger.LogError(ex, "Error occurred while adding user: " + ex.Message);
                throw new UserServiceException("Error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                throw new UserServiceException("Error occurred while adding user." + ex.Message, ex);
            }
        }

        public async Task<User> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting user with key: {key}");
                var user = await GetByKey(key);
                _context.Remove(user);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User deleted successfully.");
                return user;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                throw new UserServiceException("Error occurred while deleting user." + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retrieving all users...");
                var users = await _context.Users.ToListAsync();
                if (users.Count <= 0)
                {
                    _logger.LogWarning("There are no users present.");
                    throw new NotPresentException("There are no users present.");
                }
                _logger.LogInformation("Users retrieved successfully.");
                return users;
            }
            catch (NotPresentException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                throw new UserServiceException("Error occurred while retrieving users: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                throw new UserServiceException("Error occurred while retrieving users." + ex.Message, ex);
            }
        }

        public async Task<User> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation($"Retrieving user with key: {key}");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == key);
                if (user == null)
                {
                    _logger.LogWarning("No such user is present.");
                    throw new NotPresentException("No such user is present.");
                }
                _logger.LogInformation("User retrieved successfully.");
                return user;
            }
            catch (NotPresentException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user.");
                throw new UserServiceException("Error occurred while retrieving user: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user.");
                throw new UserServiceException("Error occurred while retrieving user." + ex.Message, ex);
            }
        }

        public async Task<User> Update(User item)
        {
            try
            {
                _logger.LogInformation($"Updating user with key: {item.UserId}");
                var user = await GetByKey(item.UserId);
                _context.Entry(user).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User updated successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                throw new UserServiceException("Error occurred while updating user." + ex.Message, ex);
            }
        }
    }
}
