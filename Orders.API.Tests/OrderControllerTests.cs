using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Controllers;
using Orders.API.Managers;
using Orders.API.Models;
using System;
using Xunit;

namespace Orders.API.Tests
{
    public class OrderControllerTests
    {
        private class Arrangement
        {
            public OrdersController SUT { get; }

            public UserDetailsApiModel Userdetails { get; }

            public Arrangement(UserDetailsApiModel userdetails, 
                               IOrderManager orderManager, 
                               IUserManager userManager)
            {
                Userdetails = userdetails;

                SUT = new OrdersController(orderManager, userManager);
            }
        }

        private class ArrangmentBuilder
        {
            private UserDetailsApiModel userdetails;
            private UserManagerStub userManager = new UserManagerStub();
            private OrderManagerStub orderManager = new OrderManagerStub();

            public ArrangmentBuilder WithUserDetails()
            {
                userdetails = new UserDetailsApiModel()
                {
                    FirstName = "First",
                    Surname = "Second"
                };

                return this;
            }

            public ArrangmentBuilder WithStock(bool inStock)
            {
                orderManager.OrderInStock = inStock;
                return this;
            }

            public ArrangmentBuilder WithUser(bool user)
            {
                userManager.UserExistsBool = user;
                return this;
            }

            public ArrangmentBuilder WithOrders(bool pastOrders)
            {
                userManager.UserHasOrdersBool = pastOrders;
                return this;
            }

            public Arrangement Build()
            {
                return new Arrangement(userdetails, orderManager, userManager);
            }
        }

        [Fact]
        public void Ctor_NullOrderManager_ShouldThrowArguementException()
        {
            // Act
            var result = Record.Exception(() => new OrdersController(null, new UserManagerStub()));

            // Assert
            result.Message.Should().Be("Value cannot be null.\r\nParameter name: orderManager");
        }

        [Fact]
        public void Ctor_NullUserManager_ShouldThrowArguementException()
        {
            // Act
            var result = Record.Exception(() => new OrdersController(new OrderManagerStub(), null));

            // Assert
            result.Message.Should().Be("Value cannot be null.\r\nParameter name: userManager");
        }

        [Fact]
        public void RegisterUser_WithDetails_ShouldReturnId()
        {
            // Arranage
            var arrangement = new ArrangmentBuilder()
                .WithUserDetails()
                .Build();

            // Act
            var result = arrangement.SUT.RegisterUser(arrangement.Userdetails);

            result.Should().BeOfType<OkObjectResult>();

            var resultObject = result as OkObjectResult;
            resultObject.Value.Should().BeOfType<Guid>();
        }

        [Fact]
        public void Checkout_WithOrderInStock_ShouldReturnOk()
        {
            // Arrange
            var arrangement = new ArrangmentBuilder()
                .WithStock(true)
                .Build();

            // Act 
            var result = arrangement.SUT.Checkout(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Checkout_WithOrderNotInStock_ShouldReturnBadResult()
        {
            // Arrange
            var arrangement = new ArrangmentBuilder()
                .WithStock(false)
                .Build();

            // Act 
            var result = arrangement.SUT.Checkout(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("Sorry these items are out of stock : error");
        }

        [Fact]
        public void GetOrders_WithUser_ShouldReturnOrderDetails()
        {
            // Arrange
            var arrangement = new ArrangmentBuilder()
                .WithUser(true)
                .WithOrders(true)
                .Build();

            // Act 
            var result = arrangement.SUT.GerOrders(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var resultObject = result as OkObjectResult;
            resultObject.Value.Should().BeOfType<OrderDetails>();
        }

        [Fact]
        public void GetOrders_WithOutUser_ShouldReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangmentBuilder()
                .WithUser(false)
                .Build();

            // Act 
            var result = arrangement.SUT.GerOrders(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("This user doesn't exist");
        }

        [Fact]
        public void GetOrders_WithOutOrders_ShouldReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangmentBuilder()
                .WithUser(true)
                .WithOrders(false)
                .Build();

            // Act 
            var result = arrangement.SUT.GerOrders(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("This user doesn't have any orders");
        }
    }
}
