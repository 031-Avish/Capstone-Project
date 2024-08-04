namespace PizzaStoreApp.Interfaces
{
    public class PaymentReturnDTO
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}