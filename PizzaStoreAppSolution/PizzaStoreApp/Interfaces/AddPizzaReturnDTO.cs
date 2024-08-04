namespace PizzaStoreApp.Interfaces
{
    public class AddPizzaReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsVegetarian { get; set; }

        public bool IsAvailable { get; set; } 
        public DateTime CreatedAt { get; set; }

    }
}