using PizzaStoreApp.Models.DTOs.loginRegisterDTO;

namespace PizzaStoreApp.Interfaces
{
    public interface IUserService
    {
        public Task<LoginReturnDTO> Login(UserLoginDTO userLoginDTO);
        public Task<UserRegisterReturnDTO> Register(UserRegisterDTO userRegisterDTO);
    }
}
