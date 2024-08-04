using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Models.DTOs.loginRegisterDTO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing user operations, including login and registration.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepo;
        private readonly IUserDetailRepository _userDetailRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepo">Repository for managing user data.</param>
        /// <param name="userDetailRepo">Repository for managing user details data.</param>
        /// <param name="tokenService">Service for generating tokens.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        public UserService(
            IRepository<int, User> userRepo,
            IUserDetailRepository userDetailRepo,
            ITokenService tokenService,
            ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _userDetailRepo = userDetailRepo;
            _tokenService = tokenService;
            _logger = logger;
        }

        #region Login Method

        /// <summary>
        /// Authenticates a user and returns a login token if successful.
        /// </summary>
        /// <param name="userLoginDTO">DTO containing user login credentials.</param>
        /// <returns>A <see cref="LoginReturnDTO"/> with the login details.</returns>
        /// <exception cref="UnableToLoginException">Thrown when an error occurs during login.</exception>
        public async Task<LoginReturnDTO> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                _logger.LogInformation("Login attempt from {Email}", userLoginDTO.Email);

                var userDB = await _userDetailRepo.GetByEmail(userLoginDTO.Email);
                if (userDB == null)
                {
                    _logger.LogWarning("Invalid username or password for {Email}", userLoginDTO.Email);
                    throw new UnauthorizedUserException("Invalid username or password.");
                }

                // Hash the password
                using (HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey))
                {
                    var encryptedPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));
                    if (ComparePassword(encryptedPass, userDB.Password))
                    {
                        _logger.LogInformation("Login successful for {Email}", userLoginDTO.Email);
                        var user = await _userRepo.GetByKey(userDB.UserId);
                        return MapUserToLoginReturnDTO(user);
                    }
                }

                _logger.LogWarning("Invalid username or password for {Email}", userLoginDTO.Email);
                throw new UnauthorizedUserException("Invalid username or password.");
            }
            catch (UnauthorizedUserException ex)
            {
                _logger.LogError(ex, "UnauthorizedUserException for user email {Email}.", userLoginDTO.Email);
                throw;
            }
            catch (NotPresentException ex)
            {
                _logger.LogError(ex, "NotPresentException for user email {Email}.", userLoginDTO.Email);
                throw;
            }
            catch (UserDetailRepositoryException ex)
            {
                _logger.LogError(ex, "UserDetailRepositoryException for user email {Email}.", userLoginDTO.Email);
                throw;
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "UserServiceException for user email {Email}.", userLoginDTO.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while logging in user with email {Email}.", userLoginDTO.Email);
                throw new UnableToLoginException("Unable to log in user at this moment.", ex);
            }
        }

        #endregion

        #region Register Method

        /// <summary>
        /// Registers a new user and returns user details upon successful registration.
        /// </summary>
        /// <param name="userRegisterDTO">DTO containing user registration details.</param>
        /// <returns>A <see cref="UserRegisterReturnDTO"/> with the registered user details.</returns>
        /// <exception cref="UnableToRegisterException">Thrown when an error occurs during registration.</exception>
        public async Task<UserRegisterReturnDTO> Register(UserRegisterDTO userRegisterDTO)
        {
            User user = null;
            UserDetail userDetail = null;

            try
            {
                _logger.LogInformation("Registering user {Email}", userRegisterDTO.Email);

                user = GenerateUser(userRegisterDTO);
                userDetail = MapUserRegisterDTOToUserDetail(userRegisterDTO);
                user = await _userRepo.Add(user);
                userDetail.UserId = user.UserId;
                userDetail = await _userDetailRepo.Add(userDetail);

                return MapUserToUserRegisterReturnDTO(user);
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "UserServiceException for email {Email}.", userRegisterDTO.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering user with email {Email}.", userRegisterDTO.Email);

                // Revert in case of any exception 
                if (user != null)
                {
                    await RevertUserInsert(user);
                }

                if (userDetail != null)
                {
                    await RevertUserDetailInsert(userDetail);
                }

                throw new UnableToRegisterException("Unable to register user at this moment.", ex);
            }
        }

        #endregion

        #region Private Methods

        private LoginReturnDTO MapUserToLoginReturnDTO(User user)
        {
            return new LoginReturnDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = _tokenService.GenerateToken(user),
                Role = user.Role
            };
        }

        private bool ComparePassword(byte[] encryptedPass, byte[] password)
        {
            return encryptedPass.SequenceEqual(password);
        }

        private UserRegisterReturnDTO MapUserToUserRegisterReturnDTO(User user)
        {
            return new UserRegisterReturnDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone
            };
        }

        private UserDetail MapUserRegisterDTOToUserDetail(UserRegisterDTO userRegisterDTO)
        {
            using (HMACSHA512 hMACSHA512 = new HMACSHA512())
            {
                return new UserDetail
                {
                    PasswordHashKey = hMACSHA512.Key,
                    Email = userRegisterDTO.Email,
                    Password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDTO.Password))
                };
            }
        }

        private User GenerateUser(UserRegisterDTO userRegisterDTO)
        {
            return new User
            {
                Name = userRegisterDTO.Name,
                Email = userRegisterDTO.Email,
                Phone = userRegisterDTO.Phone
            };
        }

        private async Task RevertUserDetailInsert(UserDetail userDetail)
        {
            _logger.LogWarning("Reverting user detail insert for user ID {UserId}.", userDetail.UserId);
            await _userDetailRepo.DeleteByKey(userDetail.UserId);
        }

        private async Task RevertUserInsert(User user)
        {
            _logger.LogWarning("Reverting user insert for user ID {UserId}.", user.UserId);
            await _userRepo.DeleteByKey(user.UserId);
        }

        #endregion
    }
}
