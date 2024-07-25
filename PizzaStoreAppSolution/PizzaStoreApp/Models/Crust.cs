using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApp.Models
{
    public class Crust
    {
        [Key]
        public int CrustId { get; set; }
        [Required]
        [MaxLength(50)]
        public string CrustName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal PriceMultiplier { get; set; }
    }
}
