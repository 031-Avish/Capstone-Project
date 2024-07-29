namespace PizzaStoreApp.Interfaces
{
    public class BeverageReturnDTO
    {
        public int BeverageId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public bool IsAvailable { get; set; }
    }
}