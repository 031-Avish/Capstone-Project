using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        
        public int? PizzaId { get; set; }
        [ForeignKey("PizzaId")]
        public Pizza? Pizza { get; set; }
        public int? SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size? Size { get; set; }
        public int? CrustId { get; set; }
        [ForeignKey("CrustId")]
        public Crust? Crust { get; set; }
        
        public int? BeverageId { get; set; }
        [ForeignKey("BeverageId")]

        public Beverage? Beverage { get; set; }
        [Range(1, int.MaxValue)]

        public int Quantity { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal SubTotal { get; set; }
        public decimal DiscountPercent { get; set; } = 0;
        public ICollection<OrderTopping> OrderToppings { get; set; }
    }
}
