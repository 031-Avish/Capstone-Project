using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PizzaStoreApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepo;
        private readonly IUserDetailRepository _userDetailRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<int, User> userRepo, IUserDetailRepository userDetailRepo, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _userDetailRepo = userDetailRepo;
            _tokenService = tokenService;
            _logger = logger;
        }
        public async Task<LoginReturnDTO> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                _logger.LogInformation("Login attempt from {0}", userLoginDTO.Email);
                var userDB = await _userDetailRepo.GetByEmail(userLoginDTO.Email);
                // hash the password
                using (HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey))
                {
                    var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));
                    //compare the password
                    bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
                    if (isPasswordSame)
                    {
                        _logger.LogInformation("Login successful for {0}", userLoginDTO.Email);
                        var user = await _userRepo.GetByKey(userDB.UserId);
                        LoginReturnDTO loginReturnDTO = MapUserToLoginReturnDTO(user);
                        return loginReturnDTO;
                    }
                }
                _logger.LogWarning("Invalid username or password for {0}", userLoginDTO.Email);
                throw new UnauthorizedUserException("Invalid username or password.");
            }
            catch (UnauthorizedUserException ex)
            {
                _logger.LogError(ex, "UnauthorizedUserException for user email {Email}.", userLoginDTO.Email);
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
                throw new UnableToLoginException("Not able to log in user at this moment.", ex);
            }
        }

        private LoginReturnDTO MapUserToLoginReturnDTO(User user)
        {
            var returnDTO = new LoginReturnDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = _tokenService.GenerateToken(user),
                Role = user.Role
            };
            return returnDTO;   
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<UserRegisterReturnDTO> Register(UserRegisterDTO userRegisterDTO)
        {
            User user=null;
            UserDetail userDetail = null;
            try
            {
                _logger.LogInformation("Registering user {0}", userRegisterDTO.Email);
                user = GenerateUser(userRegisterDTO);
                userDetail= MapUserRegisterDTOToUserDetail(userRegisterDTO);
                user = await _userRepo.Add(user);
                userDetail.UserId = user.UserId;
                userDetail = await _userDetailRepo.Add(userDetail);

                UserRegisterReturnDTO userRegisterReturnDTO = MapUserToUserRegisterReturnDTO(user);
                return userRegisterReturnDTO;
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "UserRepositoryException for email {Email}.", userRegisterDTO.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering user with email {Email}.", userRegisterDTO.Email);
                // revert in case of any exception 
                if (user != null)
                {
                    await RevertUserInsert(user);
                }

                if (userDetail != null)
                {
                    await RevertUserDetailInsert(userDetail);
                }

                throw new UnableToRegisterException("Not able to register user at this moment.", ex);
            }
        }

        private UserRegisterReturnDTO MapUserToUserRegisterReturnDTO(User user)
        {
            var returnDTO = new UserRegisterReturnDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone
            };
            return returnDTO;
        }

        private UserDetail? MapUserRegisterDTOToUserDetail(UserRegisterDTO userRegisterDTO)
        {
            using (HMACSHA512 hMACSHA512 = new HMACSHA512())
            {
                var userDetail = new UserDetail
                {
                    PasswordHashKey = hMACSHA512.Key,
                    Email = userRegisterDTO.Email,
                    Password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDTO.Password))
                };
                return userDetail;
            }
        }

        private User? GenerateUser(UserRegisterDTO userRegisterDTO)
        {
            var user = new User
            {
                Name = userRegisterDTO.Name,
                Email = userRegisterDTO.Email,
                Phone = userRegisterDTO.Phone
            };
            return user;
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
    }
}
