namespace PizzaStoreApp.Interfaces
{
    public class AddOrderDTO
    {
        public int UserId { get; set; }
        public int CartId { get; set; }
        public string? DeliveryAddress { get; set; }
        public bool IsPickup { get; set; }

    }
}