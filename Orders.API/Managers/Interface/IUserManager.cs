using Orders.API.Models;
using System;

namespace Orders.API.Managers
{
    public interface IUserManager
    {
        bool UserExist(Guid userId);
        bool UserHasOrders(Guid userId);
        bool AddOrder(Guid userId, Guid orderId);
        void AddUser(UserDetailsModel userDetails);
        OrderDetails GetOrders(Guid userId);
    }
}
