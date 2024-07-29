namespace PizzaStoreApp.Interfaces
{
    public class OrderToppingReturnDTO
    {
        public int OrderToppingId { get; set;   }
        public int ToppingId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}