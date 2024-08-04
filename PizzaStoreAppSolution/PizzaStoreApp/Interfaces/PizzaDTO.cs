namespace PizzaStoreApp.Interfaces
{
    public class PizzaDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public IFormFile ImageUrl { get; set; }
        public bool IsVegetarian { get; set; }
    }
}