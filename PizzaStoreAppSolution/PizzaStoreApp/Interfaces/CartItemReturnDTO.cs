using PizzaStoreApp.Services;

namespace PizzaStoreApp.Interfaces
{
    public class CartItemReturnDTO
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public CartPizzaReturnDTO? Pizza { get; set; }
        public CrustReturnDTO? Crust { get; set; }
        public SizeReturnDTO? Size { get; set; }
        public BeverageReturnDTO? Beverage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Decimal DiscountPercent { get; set; }
        public List<CartToppingReturnDTO>? Topping { get; set; }
       
    }
}