using Messages;
using Orders.API.Managers;
using System;
using System.Collections.Generic;

namespace Orders.API.Tests
{
    public class CatalogManagerStub : ICatalogManager
    {
        public void AddItems(Dictionary<string, int> items)
        {
            throw new NotImplementedException();
        }

        public int GetStock(string key) => 5;

        public int ItemPrice(string key) =>  ItemPrices.Prices[key];

        public void UpdateCatalogStock(Dictionary<string, int> catalogItems)
        {
        }
    }
}
