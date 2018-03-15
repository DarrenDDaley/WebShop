using FluentAssertions;
using Orders.API.Managers;
using Orders.API.Models;
using System;
using System.Linq;
using Xunit;

namespace Orders.API.Tests
{
    public class UserManagerTests
    {
        private class Arrangement
        {
            public Guid UserId { get; }

            public UserManager SUT { get; }

            public Arrangement(bool orders,
                               bool existingUser,
                               IOrderManager orderManager,
                               ICatalogManager catalogManager)
            {
                SUT = new UserManager(orderManager, catalogManager);

                if (existingUser)
                {
                    var user = new UserDetailsModel()
                    {
                        FirstName = "First",
                        Surname = "Second",
                        UserId = UserId = Guid.NewGuid()
                    };

                    SUT.AddUser(user);
                }

                if(orders)
                {
                    SUT.AddOrder(UserId, Guid.NewGuid());
                }
            }
        }

        private class ArrangementBuilder
        {
            private bool orders;
            private bool existingUser;
            private OrderManagerStub orderManager = new OrderManagerStub();
            private CatalogManagerStub catalogManager = new CatalogManagerStub();

            public ArrangementBuilder WithExistingUser(bool existingUser)
            {
                this.existingUser = existingUser;
                return this;
            }

            public ArrangementBuilder WithOrders(bool orders)
            {
                this.orders = orders;
                return this;
            }

            public Arrangement Build()
            {
                return new Arrangement(orders, existingUser, orderManager, catalogManager);
            }
        }

        [Fact]
        public void Ctor_NullOrderManger_ShouldThrowArguementNullException()
        {
            // Act 
            var error = Record.Exception(() => new UserManager(null, new CatalogManagerStub()));

            // Assert
            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: orderManager");
        }

        [Fact]
        public void Ctor_NullCatalogManager_ShouldThrowArguementNullException()
        {
            // Act 
            var error = Record.Exception(() => new UserManager(new OrderManagerStub(), null));

            // Assert
            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: catalogManager");
        }

        [Fact]
        public void UserExists_OnRecord_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingUser(true)
                .Build();

            // Act 
            var result = arrangement.SUT.UserExist(arrangement.UserId);

            // Assert
            result.Should().BeTrue();

        }


        [Fact]
        public void UserExists_NotOnRecord_ShouldReturnFalse()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingUser(false)
                .Build();

            // Act 
            var result = arrangement.SUT.UserExist(arrangement.UserId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void OrdersExist_OnRecord_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingUser(true)
                .WithOrders(true)
                .Build();

            // Act 
            var result = arrangement.SUT.UserHasOrders(arrangement.UserId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void OrdersExist_NotOnRecord_ShouldReturnFalse()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingUser(true)
                .WithOrders(false)
                .Build();

            // Act 
            var result = arrangement.SUT.UserHasOrders(arrangement.UserId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AddOrder_WithCorrectUserId_ShouldAdd()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
             .WithExistingUser(true)
             .WithOrders(false)
             .Build();

            // Act 
            var result = arrangement.SUT.AddOrder(arrangement.UserId, Guid.NewGuid());

            // Assert 
            result.Should().BeTrue();
        }

        [Fact]
        public void AddOrder_WithInCorrectUserId_ShouldAdd()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
             .WithExistingUser(false)
             .WithOrders(false)
             .Build();

            // Act 
            var result = arrangement.SUT.AddOrder(arrangement.UserId, Guid.NewGuid());

            // Assert 
            result.Should().BeFalse();
        }

        [Fact]
        public void GetOrders_WithCorrectDetails_ShouldGetOrders()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
             .WithExistingUser(true)
             .WithOrders(true)
             .Build();

            // Act 
            var result = arrangement.SUT.GetOrders(arrangement.UserId);

            // Assert 
            result.Should().BeOfType<OrderDetails>();

            result.FirstName.Should().Be("First");
            result.Surname.Should().Be("Second");

            result.Orders.First().Items.First().Key.Should().Be("Macbook");
            result.Orders.First().Items.First().Value.Should().Be(2);
            result.Orders.First().TotalPrice.Should().Be(400);
        }

        [Fact]
        public void GetOrders_UserDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
             .WithExistingUser(false)
             .WithOrders(true)
             .Build();

            // Act 
            var error = Record.Exception(() => arrangement.SUT.GetOrders(arrangement.UserId));

            // Assert
            error.Should().BeOfType<Exception>();
            error.Message.Should().Be("User doesn't exist.");
        }

        [Fact]
        public void GetOrders_OrdersDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
             .WithExistingUser(true)
              .WithOrders(false)
             .Build();

            // Act 
            var error = Record.Exception(() => arrangement.SUT.GetOrders(arrangement.UserId));

            // Assert
            error.Should().BeOfType<Exception>();
            error.Message.Should().Be("User doesn't have orders.");
        }
    }
}
