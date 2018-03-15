using Messages;
using System.Collections.Generic;

namespace Orders.API.Managers
{
    public interface ICatalogManager
    {
        int GetStock(string key);
        int ItemPrice(string key);
        void AddItems(Dictionary<string, int> items);
        void UpdateCatalogStock(Dictionary<string, int> catalogItems);
    }
}