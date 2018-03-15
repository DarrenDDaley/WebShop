using NServiceBus;
using System.Collections.Generic;

namespace Messages
{
    public class CatalogUpdated : IEvent
    {
        public Dictionary<string, int> Items { get; set; } =
          new Dictionary<string, int>();
    }
}
