using System;
using System.Threading.Tasks;
using Cart.API.Managers;
using Cart.API.Models;
using Messages;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Cart.API.Controllers
{
    [ValidateModel]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        ICartManager cartManager;
        IEndpointInstance endpoint;

        public CartController(ICartManager cartManager, IEndpointInstance endpoint)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.cartManager = cartManager ?? throw new ArgumentNullException(nameof(cartManager));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if(!cartManager.CartExist(id)) {
                return BadRequest("The cart specified doesn't exist.");
            }

            return Ok(cartManager.CartReturn(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody]CartApiModel cart)
        {
            var cartModel = new CartModel() { Items = cart.Items };

            if (!cartManager.CartValid(cartModel)) {
                return BadRequest("Your cart has an invalid item.");
            }

            var id = cartManager.AddCart(cartModel);

            await PublishEvent(cartManager.RetrieveCart(id));
            return Ok(id);
        }

        [HttpPut]
        [Route("add/{id}")]
        public async Task<IActionResult> AddToCartAsync(Guid id, [FromBody]CartApiModel cart)
        {
            var cartModel = new CartModel() { Items = cart.Items };

            if(!cartManager.CartValid(cartModel)) {
                return BadRequest("Your cart has an invalid item.");
            }

            if (cartManager.AddItemsToCart(id, cartModel))
            {
                await PublishEvent(cartManager.RetrieveCart(id));
                return Ok();
            }

            return BadRequest("The cart items specified dont exist.");
        }

        [HttpPut]
        [Route("remove/{id}")]
        public async Task<IActionResult> RemoveFromCartAsync(Guid id, [FromBody]CartApiModel cart)
        {
            var cartModel = new CartModel() { Items = cart.Items };

            if (!cartManager.CartValid(cartModel)) {
                return BadRequest("Your cart has an invalid item.");
            }

            if (cartManager.RemoveItemsFromCart(id, cartModel))
            {
                await PublishEvent(cartManager.RetrieveCart(id));
                return Ok();
            }

            return BadRequest("The cart items specified dont exist.");
        }

        private async Task PublishEvent(CartModel model)
        {
            var message = new PlaceOrder()
            {
                Id = model.Id,
                Items = model.Items
            };

            await endpoint.Send(message);
        }
    }
}