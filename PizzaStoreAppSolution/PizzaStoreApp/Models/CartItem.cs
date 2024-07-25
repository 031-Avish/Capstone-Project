using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public int? PizzaId { get; set; }
        [ForeignKey("PizzaId")]
        public Pizza Pizza { get; set; }
        public int? BeverageId { get; set; }
        [ForeignKey("BeverageId")]
        public Beverage Beverage { get; set; }
        public int? SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }
        public int? CrustId { get; set; }
        [ForeignKey("CrustId")]
        public Crust Crust { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal SubTotal { get; set; }
    }
}
