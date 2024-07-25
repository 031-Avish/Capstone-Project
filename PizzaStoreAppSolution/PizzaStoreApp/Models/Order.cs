using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDetail User { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
        [Required]
        public bool IsPickup { get; set; } // True for pickup, false for delivery
        public string? DeliveryAddress { get; set; } // Null if pickup
    }
}
