using Messages;
using NServiceBus;
using Orders.API.Managers;
using System;
using System.Threading.Tasks;

namespace Orders.API.Handlers
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        IOrderManager orderManager;

        public PlaceOrderHandler(IOrderManager orderManager)
        {
            this.orderManager = orderManager ?? 
                throw new ArgumentNullException(nameof(orderManager));
        }

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            if(orderManager.OrderExists(message.Id)) {
                orderManager.ModifyOrder(message.Id, message.Items);
            }
            else {
                orderManager.AddOrder(message.Id, message.Items);
            }

            return Task.CompletedTask;
        }
    }
}
