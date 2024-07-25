using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string SizeName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal SizeMultiplier { get; set; }
    }
}
