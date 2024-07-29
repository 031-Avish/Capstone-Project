using PizzaStoreApp.Models;

namespace PizzaStoreApp.Interfaces
{
    public class CartReturnDTO
    {
        public int CartId { get; set; }
        public List<CartItemReturnDTO> CartItems { get; set; }
        public Decimal Total { get; set; }

    }
}