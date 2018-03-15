using Messages;
using System.Collections.Generic;

namespace Cart.API.Managers
{
    public interface ICatalogManager
    {
        int ItemPrice(string key);
        Dictionary<string, int> GetItems();
        void AddItems(Dictionary<string, int> items);
    }
}
