using Cart.API.Managers;
using Cart.API.Models;
using System;

namespace Cart.API.Tests
{
    public class CartManagerStub : ICartManager
    {
        public bool CartExists { get; set; }
        public bool ValidCart { get; set; }
        public bool RemoveCartExists { get; set; }
        public bool AddItemCartExists { get; set; }

        public Guid AddCart(CartModel cart) => Guid.NewGuid();

        public bool AddItemsToCart(Guid id, CartModel cart) => AddItemCartExists;

        public bool CartExist(Guid id) => CartExists;

        public CartReturnModel CartReturn(Guid id) => new CartReturnModel();

        public bool CartValid(CartModel cart) => ValidCart;

        public bool RemoveItemsFromCart(Guid id, CartModel cart) => RemoveCartExists;

        public CartModel RetrieveCart(Guid id) => new CartModel();
    }
}
