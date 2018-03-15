using System;
using System.Collections.Generic;

namespace Orders.API.Managers
{
    public interface IOrderManager
    {
        void PlaceOrder(Guid orderId);
        bool OrderExists(Guid orderId);
        Dictionary<string, int> GetOrder(Guid orderId);
        IEnumerable<string> IsOrderInStock(Guid orderId);
        void AddOrder(Guid orderId, Dictionary<string, int> order);
        void ModifyOrder(Guid orderId, Dictionary<string, int> order);
    }
}