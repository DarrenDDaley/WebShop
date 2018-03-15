using FluentAssertions;
using Orders.API.Managers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Orders.API.Tests
{
    public class CatalogManagerTests
    {
        private class Arrangement
        {
            public CatalogManager SUT { get; }

            private Dictionary<string, int> items = new Dictionary<string, int>();

            public Arrangement()
            {
                var endpoint = new EndpointStub();

                SUT = new CatalogManager(endpoint);

                items.Add("Macbook", 10);
                items.Add("iPad", 5);

                SUT.AddItems(items);
            }
        }

        [Fact]
        private void Ctor_NullEndpoint_ThrowsExceptionError()
        {
            // Act 
            var result = Record.Exception(() => new CatalogManager(null));

            // Assert
            result.Should().BeOfType<ArgumentNullException>();
            result.Message.Should().Be("Value cannot be null.\r\nParameter name: endpoint");
        }

        [Fact]
        private void ItemPrice_WithValidKey_ShouldReturnPrice()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            var result = arrangement.SUT.ItemPrice("Macbook");

            // Assert
            result.Should().Be(200);
        }

        [Fact]
        private void ItemPrice_WithInValidKey_ShouldThrowException()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            var result = Record.Exception(()=> arrangement.SUT.ItemPrice("iPhone"));

            // Assert
            result.Should().BeOfType<Exception>();
            result.Message.Should().Be("Item does not exist");
        }

        [Fact]
        private void GetStock_WithValidKey_ShouldReturnPrice()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            var result = arrangement.SUT.GetStock("Macbook");

            // Assert
            result.Should().Be(10);
        }

        [Fact]
        private void GetStock_WithInValidKey_ShouldThrowException()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            var result = Record.Exception(() => arrangement.SUT.GetStock("iPhone"));

            // Assert
            result.Should().BeOfType<Exception>();
            result.Message.Should().Be("Item does not exist");
        }

        [Fact]
        private void AddItems_WithValidItems_ShouldBeAdded()
        {
            // Arrange
            var arrangement = new Arrangement();

            var items = new Dictionary<string, int>();
            items.Add("Macbook", 5);

            // Act
            arrangement.SUT.AddItems(items);

            // Assert
            arrangement.SUT.CatalogItems.Should().BeEquivalentTo(items);
        }

        [Fact]
        private void AddItems_WithNullItems_ShouldBeAdded()
        {
            // Arrange
            var arrangement = new Arrangement();

            var items = new Dictionary<string, int>();
            items.Add("Macbook", 5);

            // Act
            var result = Record.Exception(() => arrangement.SUT.AddItems(null));
            

            // Assert
            result.Should().BeOfType<Exception>();
            result.Message.Should().Be("Items cant be empty");
        }

        [Fact]
        private void UpdateCatalogStock_WithValidItems_ShouldBeAdded()
        {
            // Arrange
            var arrangement = new Arrangement();

            var items = new Dictionary<string, int>();
            items.Add("Macbook", 5);

            // Act
            arrangement.SUT.UpdateCatalogStock(items);

            // Assert
            arrangement.SUT.CatalogItems["Macbook"].Should().Be(5);
        }

        [Fact]
        private void UpdateCatalogStock_WithNullItems_ShouldBeAdded()
        {
            // Arrange
            var arrangement = new Arrangement();

            var items = new Dictionary<string, int>();
            items.Add("Macbook", 5);

            // Act
            var result = Record.Exception(() => arrangement.SUT.UpdateCatalogStock(null));


            // Assert
            result.Should().BeOfType<Exception>();
            result.Message.Should().Be("Items cant be empty");
        }
    }
}
