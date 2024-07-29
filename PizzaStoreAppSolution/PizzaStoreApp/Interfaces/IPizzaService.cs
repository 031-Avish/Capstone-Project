using PizzaStoreApp.Models;

namespace PizzaStoreApp.Interfaces
{
    public interface IPizzaService
    {
        public Task<List<PizzaReturnDTO>> GetAllPizzawithSizeAndCrust();
        public Task<PizzaReturnDTO> GetPizzaByPizzaId(int pizzaId);
        public Task<List<PizzaReturnDTO>> GetAllNewPizza();
        public Task<List<PizzaReturnDTO>> GetAllVegetarianPizza();
        public Task<List<PizzaReturnDTO>> GetMostSoldPizza();
    }
}
