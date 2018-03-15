using Orders.API.Managers;
using System;
using System.Collections.Generic;

namespace Orders.API.Tests
{
    public class OrderManagerStub : IOrderManager
    {
        private Dictionary<string, int> orders = new Dictionary<string, int>();

        public bool OrderExistsBool { get; set; }

        public bool OrderInStock { get; set; }

        public void AddOrder(Guid orderId, Dictionary<string, int> order) { }

        public Dictionary<string, int> GetOrder(Guid orderId)
        {
            orders.Add("Macbook", 2);
            return orders;
        }

        public IEnumerable<string> IsOrderInStock(Guid orderId) {
            return OrderInStock ? new List<string>() : new List<string>() { "error" };
        }

        public void ModifyOrder(Guid orderId, Dictionary<string, int> order) { }

        public bool OrderExists(Guid orderId) => OrderExistsBool;

        public void PlaceOrder(Guid orderId) { }
    }
}
