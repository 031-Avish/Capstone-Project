namespace PizzaStoreApp.Interfaces
{
    public class OrderReturnDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<OrderItemReturnDTO> OrderItems { get; set; }
        public Decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
    }
}