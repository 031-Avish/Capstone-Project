using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaStoreApp.Models
{
    public class CartItemTopping
    {
        [Key]
        public int CartItemToppingId { get; set; }
        
        public int CartItemId { get; set; }
        [ForeignKey("CartItemId")]
        public CartItem CartItem { get; set; }
    
        public int ToppingId { get; set; }
        [ForeignKey("ToppingId")]
        public Topping Topping { get; set; }

        public int Quantity { get; set; }
    }
}
