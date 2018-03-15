using Messages;
using System.Collections.Generic;

namespace Cart.API.Managers
{
    public class CatalogManager : ICatalogManager
    {
        private Dictionary<string, int> items { get; set; } =
           new Dictionary<string, int>();

        public Dictionary<string, int> GetItems() => items;
        public void AddItems(Dictionary<string, int> items) => this.items = items;
        public int ItemPrice(string key) => ItemPrices.Prices[key];
    }
}
