using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.API.Managers
{
    public class OrderManager : IOrderManager
    {
        private ICatalogManager catalogManager;

        private Dictionary<Guid, Dictionary<string, int>> orderData =
            new Dictionary<Guid, Dictionary<string, int>>();

        public OrderManager(ICatalogManager catalogManager)
        {
            this.catalogManager = catalogManager ??
                throw new ArgumentNullException(nameof(catalogManager));
        }

        public bool OrderExists(Guid orderId) => 
            orderData.Any(o => o.Key == orderId); 

        public void AddOrder(Guid orderId, Dictionary<string, int> order) => 
            orderData.Add(orderId, order);

        public void ModifyOrder(Guid orderId, Dictionary<string, int> order) =>
            orderData[orderId] = order;

        public IEnumerable<string> IsOrderInStock(Guid orderId)
        {
            var outOfStockItems = new List<string>();

            var order = orderData[orderId];

            foreach (var item in order)
            {
                if (item.Value > catalogManager.GetStock(item.Key))
                    outOfStockItems.Add(item.Key);
            }
            return outOfStockItems;
        }

        public Dictionary<string, int> GetOrder(Guid orderId) {
            return orderData[orderId];
        }

        public void PlaceOrder(Guid orderId) {
            catalogManager.UpdateCatalogStock(orderData[orderId]);
        }
    }
}
