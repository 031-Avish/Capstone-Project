namespace PizzaStoreApp.Interfaces
{
    public class ToppingReturnDTO
    {
        public int ToppingId { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string? Image { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsAvailable { get; set; }
    }
}