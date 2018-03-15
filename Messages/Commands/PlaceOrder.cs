using System;
using System.Collections.Generic;
using NServiceBus;

namespace Messages
{
    public class PlaceOrder : ICommand
    {
        public Guid Id { get; set; }
        public Dictionary<string, int> Items { get; set; } 
    }
}
