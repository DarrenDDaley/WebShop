using System;
using System.Collections.Generic;

namespace Cart.API.Models
{
    public class CartModel
    {
        public Guid Id { get; set; }
        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();
    }
}