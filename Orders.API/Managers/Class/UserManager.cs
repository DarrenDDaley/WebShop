using Orders.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.API.Managers
{
    public class UserManager : IUserManager
    {
        private IOrderManager orderManager;
        private ICatalogManager catalogManager;

        private List<UserDetailsModel> users = new List<UserDetailsModel>();
        private Dictionary<Guid, List<Guid>> orders = new Dictionary<Guid, List<Guid>>();

        public UserManager(IOrderManager orderManager, ICatalogManager catalogManager)
        {
            this.orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
            this.catalogManager = catalogManager ?? throw new ArgumentNullException(nameof(catalogManager));
        }

        public bool UserExist(Guid userId) => users.Any(u => u.UserId == userId);

        public bool UserHasOrders(Guid userId) => orders.ContainsKey(userId);

        public bool AddOrder(Guid userId, Guid orderId)
        {
            if(!UserExist(userId)) {
                return false;
            }

            if(orders.ContainsKey(userId)) {
                orders[userId].Add(orderId);
            }
            else {
                orders.Add(userId, new List<Guid>() { orderId });
            }

            return true;
        }

        public void AddUser(UserDetailsModel userDetails) =>
            users.Add(userDetails);

        public OrderDetails GetOrders(Guid userId)
        {
            if(!UserExist(userId)) {
                throw new Exception("User doesn't exist.");
            }

            if (!UserHasOrders(userId)) {
                throw new Exception("User doesn't have orders.");
            }

            var userDetails = users.SingleOrDefault(f => f.UserId == userId);
            var usersOrders = orders[userId];

            var orderDetails = new OrderDetails()
            {
                FirstName = userDetails.FirstName,
                Surname = userDetails.Surname
            };

            foreach (var order in usersOrders)
            {
                var items = orderManager.GetOrder(order);
                var totalPrice = 0;

                foreach (var item in items) {
                    totalPrice += totalPrice += catalogManager.ItemPrice(item.Key) * item.Value;
                }

                orderDetails.Orders.Add(new OrderModel { Items = items,  TotalPrice = totalPrice});
            }

            return orderDetails;
        }
    }
}
