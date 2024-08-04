using PizzaStoreApp.Services;

namespace PizzaStoreApp.Interfaces
{
    public interface IBeverageService
    {
        public Task<List<BeverageReturnDTO>> GetAllBeverages();
        public Task<BeverageReturnDTO> GetBeverageByBeverageId(int beverageId);
        public Task<BeverageReturnDTO> AddBeverage(BeverageDTO beverageDTO);

    }
}
