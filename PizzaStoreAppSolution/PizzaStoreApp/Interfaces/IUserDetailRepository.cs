using PizzaStoreApp.Models;

namespace PizzaStoreApp.Interfaces
{
    public interface IUserDetailRepository:IRepository<int, UserDetail>
    {
        Task<UserDetail> GetByEmail(string email);
    }
}
