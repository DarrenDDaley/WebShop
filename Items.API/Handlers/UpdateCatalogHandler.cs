using Items.API.Managers;
using Messages.Commands;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Items.API.Handlers
{
    public class UpdateCatalogHandler : IHandleMessages<UpdateCatalog>
    {
        ICatalogManager catalogManager;

        public UpdateCatalogHandler(ICatalogManager catalogManager)
        {
            this.catalogManager = catalogManager ??
                throw new ArgumentNullException(nameof(catalogManager));
        }


        public Task Handle(UpdateCatalog message, IMessageHandlerContext context)
        {
            catalogManager.Update(message.Items);
            return Task.CompletedTask;
        }
    }
}
