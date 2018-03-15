using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cart.API.Tests
{
    public class EndpointStub : IEndpointInstance
    {
        public Task Publish(object message, PublishOptions options) => Task.CompletedTask;

        public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions) => Task.CompletedTask;

        public Task Send(object message) => Task.CompletedTask;

        public Task Send(object message, SendOptions options) => Task.CompletedTask;

        public Task Send<T>(Action<T> messageConstructor, SendOptions options) => Task.CompletedTask;

        public Task Stop() => Task.CompletedTask;

        public Task Subscribe(Type eventType, SubscribeOptions options) => Task.CompletedTask;

        public Task Unsubscribe(Type eventType, UnsubscribeOptions options) => Task.CompletedTask;
    }
}
