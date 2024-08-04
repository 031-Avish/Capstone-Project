namespace PizzaStoreApp.Services
{
    public class BeverageDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}