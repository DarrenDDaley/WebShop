using System;
using System.Collections.Generic;
using Messages;
using Messages.Commands;
using NServiceBus;

namespace Orders.API.Managers
{
    public class CatalogManager : ICatalogManager
    {
        IEndpointInstance endpoint;

        public Dictionary<string, int> CatalogItems { get; private set; } =
          new Dictionary<string, int>();

        public CatalogManager(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        public int ItemPrice(string key)
        {
            if (!ItemPrices.Prices.ContainsKey(key)) {
                throw new Exception("Item does not exist");
            }

            return ItemPrices.Prices[key];
        }
        public int GetStock(string key)
        {
            if (!CatalogItems.ContainsKey(key)) {
                throw new Exception("Item does not exist");
            }

            return CatalogItems[key];
        }

        public void AddItems(Dictionary<string, int> catalogItems)
        {
            if(catalogItems == null || catalogItems.Count == 0) {
                throw new Exception("Items cant be empty");
            }

           this.CatalogItems = catalogItems;
        }

        public void UpdateCatalogStock(Dictionary<string, int> catalogItems)
        {
            if (catalogItems == null || catalogItems.Count == 0) {
                throw new Exception("Items cant be empty");
            }

            foreach (var item in catalogItems)
            {
                if(CatalogItems.ContainsKey(item.Key)) {
                     CatalogItems[item.Key] -= item.Value;
                }
            }

            var message = new UpdateCatalog() {
                Items = CatalogItems
            };

            endpoint.Send(message).ConfigureAwait(false);
        }
     
    }
}
