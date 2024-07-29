using PizzaStoreApp.Models;
using System.Drawing;


namespace PizzaStoreApp.Interfaces
{
    public class PizzaReturnDTO
    {
        public int PizzaId { get; set; }
        public string Name { get; set; }
        public List<SizeReturnDTO> sizes { get; set; }
        public List<CrustReturnDTO> crusts { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }  
        public DateTime CreatedAt { get; set; }

    }
}