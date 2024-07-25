using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class OrderTopping
    {
        [Key]
        public int OrderToppingId { get; set; }
        [Required]
        public int OrderDetailId { get; set; }
        [ForeignKey("OrderDetailId")]
        public OrderDetail OrderDetail { get; set; }
        [Required]
        public int ToppingId { get; set; }
        [ForeignKey("ToppingId")]
        public Topping Topping { get; set; }
    }
}
