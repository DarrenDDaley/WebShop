using Items.API.Models;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Items.API.Managers
{
    public class CatalogManager  : ICatalogManager
    {
        Dictionary<string, int> items { get; set; } = 
            new Dictionary<string, int>();

        public CatalogManager()
        {
            items.Add("Macbook", 10);
            items.Add("iPad", 5);
        }

        public void Update(Dictionary<string, int> items)
        {
            if(items == null || items.Count == 0) {
                throw new Exception("Catalog list is empty.");
            }

            foreach (var item in items)
            {
                if(!this.items.ContainsKey(item.Key)) {
                    throw new Exception("Item on the list is not part of the catalog.");
                }
            }

            this.items = items;
        }

        public Dictionary<string, int> GetAllItems() => items;

        public IEnumerable<CatalogModel> GetAllStock()
        {
            List<CatalogModel> catlogItems = new List<CatalogModel>();

            foreach (var item in items)
            {
                catlogItems.Add(new CatalogModel
                {
                    Description = item.Key,
                    Qauntity = item.Value,
                    Price = ItemPrices.Prices[item.Key]
                });
            }
            return catlogItems;
        }
    }
}
