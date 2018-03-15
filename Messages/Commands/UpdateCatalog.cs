using NServiceBus;
using System.Collections.Generic;

namespace Messages.Commands
{
    public class UpdateCatalog : ICommand
    {
        public Dictionary<string, int> Items { get; set; } =
         new Dictionary<string, int>();
    }
}
