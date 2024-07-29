namespace PizzaStoreApp.Services
{
    public class CartToppingReturnDTO
    {
        public int ToppingId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}