using System.Collections.Generic;

namespace Cart.API.Models
{
    public class CartReturnModel
    {
        public int TotalPrice { get; set; }
        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();
    }
}
