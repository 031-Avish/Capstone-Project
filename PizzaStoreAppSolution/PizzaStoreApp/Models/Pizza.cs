using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Pizza
    {
        [Key]
        public int PizzaId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        [Required]
        public bool IsAvailable { get; set; }=true;

        public bool IsVegetarian { get; set; }
        public string ImageUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now; 

    }
}
