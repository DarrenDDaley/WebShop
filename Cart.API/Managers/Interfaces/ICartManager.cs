using Cart.API.Models;
using System;

namespace Cart.API.Managers
{
    public interface ICartManager
    {
        bool CartExist(Guid id);
        Guid AddCart(CartModel cart);
        bool CartValid(CartModel cart);
        CartModel RetrieveCart(Guid id);
        CartReturnModel CartReturn(Guid id);
        bool AddItemsToCart(Guid id, CartModel cart);
        bool RemoveItemsFromCart(Guid id, CartModel cart);
    }
}
