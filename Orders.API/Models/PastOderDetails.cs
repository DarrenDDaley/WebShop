using System.Collections.Generic;

namespace Orders.API.Models
{
    public class OrderDetails
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
