using FluentAssertions;
using Items.API.Managers;
using Items.API.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Items.API.Tests
{
    public class CatalogManagerTests
    {
        private class Arrangements
        {
            public CatalogManager SUT { get; }

            public Arrangements()
            {
                SUT = new CatalogManager();
            }
        }

        [Fact]
        public void GetAllItems_ShouldReturnCorrectItems()
        {
            // Arrange
            var arrangements = new Arrangements();

            var catalogItems = new Dictionary<string, int>();
            catalogItems.Add("Macbook", 10);
            catalogItems.Add("iPad", 5);

            // Act
            var result = arrangements.SUT.GetAllItems();

            // Assert
            result.Should().BeEquivalentTo(catalogItems);
        }

        [Fact]
        public void GetAllStock_ShouldReturnCorrectStock()
        {
            // Arrange
            var arrangements = new Arrangements();

            var catalogStock = new List<CatalogModel>();
            catalogStock.Add(
                new CatalogModel
                {
                    Description = "Macbook",
                    Price = 200,
                    Qauntity = 10
                });

            catalogStock.Add(
                new CatalogModel
                {
                    Description = "iPad",
                    Price = 100,
                    Qauntity = 5
                });


            // Act 
            var result = arrangements.SUT.GetAllStock();

            // Assert
            result.Should().BeEquivalentTo(catalogStock);
        }

        [Fact]
        public void UpdateStock_WithValidStock_ShouldHaveNewStockQauntity()
        {
            // Arrange
            var arrangements = new Arrangements();

            Dictionary<string, int> newStock = new Dictionary<string, int>();
            newStock.Add("Macbook", 3);
            newStock.Add("iPad", 2);

            // Act
            arrangements.SUT.Update(newStock);

            // Assert
            newStock.Should().BeEquivalentTo(arrangements.SUT.GetAllItems());

        }

        [Fact]
        public void UpdateStock_InvalidStock_ShouldThrowException()
        {
            // Arrange
            var arrangements = new Arrangements();

            Dictionary<string, int> newStock = new Dictionary<string, int>();
            newStock.Add("Macbook", 3);
            newStock.Add("iPhone", 2);

            // Act
            var error = Record.Exception(() => arrangements.SUT.Update(newStock));

            // Assert
            error.Should().BeOfType<Exception>();
            error.Message.Should().Be("Item on the list is not part of the catalog.");

        }

        [Fact]
        public void UpdateStock_EmptyStock_ShouldThrowException()
        {
            // Arrange
            var arrangements = new Arrangements();

            Dictionary<string, int> newStock = new Dictionary<string, int>();

            // Act
            var error = Record.Exception(() => arrangements.SUT.Update(newStock));

            // Assert
            error.Should().BeOfType<Exception>();
            error.Message.Should().Be("Catalog list is empty.");
        }

        [Fact]
        public void UpdateStock_NullStock_ShouldThrowException()
        {
            // Arrange
            var arrangements = new Arrangements();

            Dictionary<string, int> newStock = null;

            // Act
            var error = Record.Exception(() => arrangements.SUT.Update(newStock));

            // Assert
            error.Should().BeOfType<Exception>();
            error.Message.Should().Be("Catalog list is empty.");
        }
    }
}
