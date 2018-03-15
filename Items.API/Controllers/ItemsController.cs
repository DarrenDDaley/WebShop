using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Messages;
using System.Threading.Tasks;
using Items.API.Managers;
using Items.API.Models;

namespace Items.API.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private IEndpointInstance endpoint;
        private ICatalogManager catalogManager;

        public ItemsController(ICatalogManager catalogManager, IEndpointInstance endpoint)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.catalogManager = catalogManager ?? throw new ArgumentNullException(nameof(catalogManager));
        }


        [HttpGet]
        public async Task<IEnumerable<CatalogModel>> Get()
        {
            var message = new CatalogUpdated() {
                Items = catalogManager.GetAllItems()
            };

            await endpoint.Publish(message)
                .ConfigureAwait(false);

            return catalogManager.GetAllStock();
        }
    }
}