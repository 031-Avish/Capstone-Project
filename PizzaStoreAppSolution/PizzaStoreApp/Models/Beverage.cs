using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Beverage
    {
        [Key]
        public int BeverageId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
