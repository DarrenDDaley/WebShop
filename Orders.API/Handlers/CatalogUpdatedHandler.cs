using Messages;
using NServiceBus;
using Orders.API.Managers;
using System;
using System.Threading.Tasks;

namespace Orders.API.Handlers
{
    public class CatalogUpdatedHandler : IHandleMessages<CatalogUpdated>
    {
        ICatalogManager catalogManager;

        public CatalogUpdatedHandler(ICatalogManager catalogManager)
        {
            this.catalogManager = catalogManager ?? 
                throw new ArgumentNullException(nameof(catalogManager));
        }

        public Task Handle(CatalogUpdated message, IMessageHandlerContext context)
        {
            catalogManager.AddItems(message.Items);
            return Task.CompletedTask;
        }
    }
}
