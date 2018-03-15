using FluentAssertions;
using Orders.API.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Orders.API.Tests
{
    public class OrderManagerTests
    {
        private class Arrangement
        {
            public OrderManager SUT { get; }

            public Guid OrderId { get; }

            public Arrangement(bool orderExists, int orderAmount, ICatalogManager catalogManager)
            {
                SUT = new OrderManager(catalogManager);

                if(orderExists)
                {
                    OrderId = Guid.NewGuid();

                    var order = new Dictionary<string, int>();
                    order.Add("Macbook", orderAmount);

                    SUT.AddOrder(OrderId, order);
                }
            }
        }

        private class ArrangementBuilder
        {
            private int orderAmount = 1;
            private bool orderExists;
            private CatalogManagerStub catalogManager = new CatalogManagerStub();

            public ArrangementBuilder WithOrder(bool orderExists)
            {
                this.orderExists = orderExists;
                return this;
            }

            public ArrangementBuilder WithOrderAmount(int orderAmount)
            {
                this.orderAmount = orderAmount;
                return this;
            }

            public Arrangement Build()
            {
                return new Arrangement(orderExists, orderAmount, catalogManager);
            }
        }

        [Fact]
        public void Ctor_NullCatalogManager_ShouldThrowArguementNullException()
        {
            // Act 
            var error = Record.Exception(() => new OrderManager(null));

            // Assert
            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: catalogManager");
        }

        [Fact]
        public void OrderExists_WithExistingOrder_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithOrder(true)
                .Build();

            // Act
            var result = arrangement.SUT.OrderExists(arrangement.OrderId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void OrderExists_WithoutExistingOrder_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithOrder(false)
                .Build();

            // Act
            var result = arrangement.SUT.OrderExists(arrangement.OrderId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderInStock_InStock_ReturnEmptyEnumberable()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
               .WithOrder(true)
               .WithOrderAmount(2)
               .Build();

            // Act 
            var result = arrangement.SUT.IsOrderInStock(arrangement.OrderId);

            // Assert
            result.Should().BeEquivalentTo(new List<string>());
        }

        [Fact]
        public void IsOrderInStock_NotInStock_ReturnEmptyEnumberable()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
               .WithOrder(true)
               .WithOrderAmount(6)
               .Build();

            // Act 
            var result = arrangement.SUT.IsOrderInStock(arrangement.OrderId);

            // Assert
            result.First().Should().Be("Macbook");
        }
    }
}
