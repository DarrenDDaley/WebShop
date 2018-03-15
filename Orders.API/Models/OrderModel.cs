using System.Collections.Generic;

namespace Orders.API.Models
{
    public class OrderModel
    {
        public int TotalPrice { get; set; }
        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();
    }
}
