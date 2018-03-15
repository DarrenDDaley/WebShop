using Orders.API.Managers;
using Orders.API.Models;
using System;

namespace Orders.API.Tests
{
    public class UserManagerStub : IUserManager
    {
        public bool UserHasOrdersBool { get; set; }

        public bool UserExistsBool { get; set; }

        public bool AddOrder(Guid userId, Guid orderId) => true;

        public void AddUser(UserDetailsModel userDetails) { }

        public OrderDetails GetOrders(Guid userId) => new OrderDetails();

        public bool UserExist(Guid userId) => UserExistsBool;

        public bool UserHasOrders(Guid userId) => UserHasOrdersBool;
    }
}
