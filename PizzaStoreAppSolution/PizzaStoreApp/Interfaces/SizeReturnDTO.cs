using PizzaStoreApp.Models;
using System.Drawing;
using Size = PizzaStoreApp.Models.Size;

namespace PizzaStoreApp.Interfaces
{
    public class SizeReturnDTO
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public decimal SizeMultiplier { get; set; }
    }
}