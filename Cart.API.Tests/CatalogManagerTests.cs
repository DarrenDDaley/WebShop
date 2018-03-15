using Cart.API.Managers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Cart.API.Tests.Stub
{
    public class CatalogManagerTests
    {
        private class Arrangement
        {
            public CatalogManager SUT { get; }
            public Dictionary<string, int> Items { get; } 


            public Arrangement()
            {
                SUT = new CatalogManager();
                Items = new Dictionary<string, int>();

                Items.Add("Macbook", 10);
                Items.Add("ipad", 5);
            }
        }

        [Fact]
        public void AddItems_ShouldBeAdded()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            arrangement.SUT.AddItems(arrangement.Items);

            // Assert
            arrangement.SUT.GetItems().Should().BeEquivalentTo(arrangement.Items);
        }

        [Fact]
        public void AddPrice_ShouldReturnPrice()
        {
            // Arrange
            var arrangement = new Arrangement();

            // Act
            var result = arrangement.SUT.ItemPrice("Macbook");

            // Assert
            result.Should().Be(200);
        }
    }
}
