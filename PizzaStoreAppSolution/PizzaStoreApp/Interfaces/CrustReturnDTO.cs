using PizzaStoreApp.Models;
using System.Drawing;
using Size = PizzaStoreApp.Models.Size;

namespace PizzaStoreApp.Interfaces
{
    public class CrustReturnDTO
    {
        public int CrustId { get; set; }
        public string CrustName { get; set; }
        public decimal PriceMultiplier { get; set; }

    }
}