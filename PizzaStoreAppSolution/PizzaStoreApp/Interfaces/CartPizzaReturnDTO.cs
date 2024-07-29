namespace PizzaStoreApp.Interfaces
{
    public class CartPizzaReturnDTO
    {
        public int PizzaId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsVegetarian { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }   
    }
}