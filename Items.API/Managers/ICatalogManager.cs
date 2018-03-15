using Items.API.Models;
using System.Collections.Generic;

namespace Items.API.Managers
{
    public interface ICatalogManager
    {
        Dictionary<string, int> GetAllItems();
        IEnumerable<CatalogModel> GetAllStock();
        void Update(Dictionary<string, int> items);
    }
}
