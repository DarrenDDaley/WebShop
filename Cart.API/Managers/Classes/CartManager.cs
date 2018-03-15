using System;
using System.Linq;
using System.Collections.Generic;
using Cart.API.Models;

namespace Cart.API.Managers
{
    public class CartManager : ICartManager
    {
        ICatalogManager catalog;
        public List<CartModel> Carts { get; } = new List<CartModel>();

        public CartManager(ICatalogManager catalog)
        {
            this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public bool CartExist(Guid id) => Carts.Any(c => c.Id == id);

        public CartModel RetrieveCart(Guid id) => Carts.FirstOrDefault(c => c.Id == id);

        public Guid AddCart(CartModel cart)
        {
            cart.Id = Guid.NewGuid();
            Carts.Add(cart);

            return cart.Id;
        }

        public bool AddItemsToCart(Guid id, CartModel cart)
        {
            var currentCart = RetrieveCart(id);

            if (currentCart == null) {
                return false;
            }

            foreach (var item in cart.Items)
            {
                if(currentCart.Items.ContainsKey(item.Key))
                {
                    currentCart.Items[item.Key] += item.Value;
                    continue;
                }
                currentCart.Items.Add(item.Key, item.Value);
            }

            return true;
        }

        public bool RemoveItemsFromCart(Guid id, CartModel cart)
        {
            var currentCart = RetrieveCart(id);

            if (currentCart == null) {
                return false;
            }

            foreach (var item in cart.Items)
            {
                if (currentCart.Items.ContainsKey(item.Key)) {
                    currentCart.Items[item.Key] -= Math.Max(0, item.Value);
                }
            }

            return true;
        }

        public bool CartValid(CartModel cart)
        {
            foreach (var key in cart.Items.Keys)
            {
                if (!catalog.GetItems().Keys.Contains(key)) {
                    return false;
                }
            }

            return true;
        }

        public CartReturnModel CartReturn(Guid id)
        {
            var currentCart = RetrieveCart(id);

            if (currentCart == null) {
                return null;
            }

            int totalPrice = 0;
            foreach (var item in currentCart.Items) {
                totalPrice += catalog.ItemPrice(item.Key) * item.Value;
            }

            return new CartReturnModel { Items = currentCart.Items, TotalPrice = totalPrice };
        }
    }
}
