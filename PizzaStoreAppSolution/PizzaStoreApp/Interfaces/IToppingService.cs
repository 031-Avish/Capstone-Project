namespace PizzaStoreApp.Interfaces
{
    public interface IToppingService
    {
        public Task<List<ToppingReturnDTO>> GetAllToppings();
        public Task<ToppingReturnDTO> GetToppingByToppingId(int toppingId);


    }
}
