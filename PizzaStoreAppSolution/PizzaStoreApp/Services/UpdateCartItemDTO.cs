namespace PizzaStoreApp.Services
{
    public class UpdateCartItemDTO
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int? PizzaId { get; set; } // Nullable for when only adding beverages
        public int? BeverageId { get; set; } // Nullable for when only adding pizzas
        public int? SizeId { get; set; } // Size of the pizza
        public int? CrustId { get; set; } // Crust of the pizza
        public int Quantity { get; set; }
        public Dictionary<int, int>? ToppingIds { get; set; }
    }
}