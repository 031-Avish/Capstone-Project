using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Topping
    {
        [Key]
        public int ToppingId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ToppingName { get; set; }
        public bool IsVegetarian { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
