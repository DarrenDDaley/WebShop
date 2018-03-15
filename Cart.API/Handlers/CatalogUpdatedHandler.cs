using Cart.API.Managers;
using Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cart.API.Handlers
{
    public class CatalogUpdatedHandler : IHandleMessages<CatalogUpdated>
    {
        private ICatalogManager catalog;

        public CatalogUpdatedHandler(ICatalogManager catalog)
        {
            this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public Task Handle(CatalogUpdated message, IMessageHandlerContext context)
        {
            catalog.AddItems(message.Items);
            return Task.CompletedTask;
        }
    }
}
