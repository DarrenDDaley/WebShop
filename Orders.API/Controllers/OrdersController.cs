using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Managers;
using Orders.API.Models;

namespace Orders.API.Controllers
{
    [ValidateModel]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        IUserManager userManager;
        IOrderManager orderManager;

        public OrdersController(IOrderManager orderManager, IUserManager userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser([FromBody] UserDetailsApiModel userDetails)
        {
            var user = new UserDetailsModel()
            {
                UserId = Guid.NewGuid(),
                FirstName = userDetails.FirstName,
                Surname = userDetails.Surname
            };

            userManager.AddUser(user);
            return Ok(user.UserId);
        }

        [HttpPut]
        [Route("checkout/{orderId}/{userId}")]
        public IActionResult Checkout(Guid orderId, Guid userId)
        {
            var itemsNotInStock = orderManager.IsOrderInStock(orderId);

            if(itemsNotInStock.Any()) {
                return BadRequest($"Sorry these items are out of stock : " 
                    + string.Join(",", itemsNotInStock.ToArray()));
            }

            orderManager.PlaceOrder(orderId);
            userManager.AddOrder(userId, orderId);

            return Ok();
        }

        [HttpGet("{userId}")]
        public IActionResult GerOrders(Guid userId)
        {
            if(!userManager.UserExist(userId)) {
                return BadRequest("This user doesn't exist");
            }

            if (!userManager.UserHasOrders(userId)) {
                return BadRequest("This user doesn't have any orders");
            }

            return Ok(userManager.GetOrders(userId));
        }
    }
}