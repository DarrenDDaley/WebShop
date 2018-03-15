using Cart.API.Managers;
using Cart.API.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cart.API.Tests
{
    public class CartManagerTests
    {
        private class Arrangement
        {
            public Guid CartId { get; }

            public CartManager SUT { get; }

            public CartModel Cart { get; set; }

            public CartModel ExistingCart { get; }

            public Arrangement(CartModel cart, bool existingCart)
            {
                Cart = cart;

                var items = new Dictionary<string, int>();
                items.Add("Macbook", 10);
                items.Add("iPad", 5);

                var catalogManager = new CatalogManager();
                catalogManager.AddItems(items);

                SUT = new CartManager(catalogManager);

                if (existingCart)
                {
                    ExistingCart = new CartModel() { Items = items };
                    CartId =  SUT.AddCart(ExistingCart);
                }
            }
        }

        private class ArrangementBuilder
        {
            bool cartExists;
            CartModel cartModel;

            public ArrangementBuilder WithStandardCart()
            {
                var items = new Dictionary<string, int>();
                items.Add("Macbook", 2);

                cartModel = new CartModel() {
                    Items = items
                };

                return this;
            }

            public ArrangementBuilder WithExistingCart(bool cartExists)
            {
                this.cartExists = cartExists;
                return this;
            }

            public Arrangement Build()
            {
                return new Arrangement(cartModel, cartExists);
            }
        }

        [Fact]
        public void AddCart_ShouldReturnId()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithStandardCart()
                .Build();

            // Act 
            var result = arrangement.SUT.AddCart(arrangement.Cart);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void CartExists_WithExistingCart_ShouldReturnTrue()
        {
            // Arrange
            var arrangment = new ArrangementBuilder()
                .WithExistingCart(true)
                .Build();

            // Act
            var result = arrangment.SUT.CartExist(arrangment.CartId);

            // Assert
            result.Should().BeTrue();

        }

        [Fact]
        public void CartExists_WithNonExistantCart_ShouldReturnFalse()
        {
            // Arrange
            var arrangment = new ArrangementBuilder()
                .WithExistingCart(false)
                .Build();

            // Act
            var result = arrangment.SUT.CartExist(arrangment.CartId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void RetriveCart_WithExistingCart_ShouldReturnCart()
        {
            // Arrange
            var arrangment = new ArrangementBuilder()
                .WithExistingCart(true)
                .Build();

            // Act
            var result = arrangment.SUT.RetrieveCart(arrangment.CartId);


            // Assert
            result.Items.Should().BeEquivalentTo(arrangment.ExistingCart.Items);
        }

        [Fact]
        public void RetriveCart_WithNonExistantCart_ShouldReturnNull()
        {
            // Arrange
            var arrangment = new ArrangementBuilder()
                .WithExistingCart(false)
                .Build();

            // Act
            var result = arrangment.SUT.RetrieveCart(arrangment.CartId);


            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void AddToExistingCart_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            var result = arrangement.SUT
            .AddItemsToCart(arrangement.CartId, arrangement.Cart);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AddToNonExistantCart_ShouldReturnFalse()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            var result = arrangement.SUT
            .AddItemsToCart(Guid.NewGuid(), arrangement.Cart);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AddToExistingCart_ShouldHaveRightAmount()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            arrangement.SUT.AddItemsToCart(arrangement.CartId, arrangement.Cart);

            var result = arrangement.SUT.RetrieveCart(arrangement.CartId);

            // Assert
            result.Should().NotBeNull();
            result.Items["iPad"].Should().Be(5);
            result.Items["Macbook"].Should().Be(12);
        }

        [Fact]
        public void RemoveFromExistingCart_ShouldReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            var result = arrangement.SUT
            .RemoveItemsFromCart(arrangement.CartId, arrangement.Cart);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void RemoveFromNonExistantCart_ShouldReturnFalse()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            var result = arrangement.SUT
            .RemoveItemsFromCart(Guid.NewGuid(), arrangement.Cart);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void RemoveFromExistingCart_ShouldHaveRightAmount()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .WithStandardCart()
                .Build();

            // Act 
            arrangement.SUT.RemoveItemsFromCart(arrangement.CartId, arrangement.Cart);

            var result = arrangement.SUT.RetrieveCart(arrangement.CartId);

            // Assert
            result.Should().NotBeNull();
            result.Items["iPad"].Should().Be(5);
            result.Items["Macbook"].Should().Be(8);
        }

        [Fact]
        public void CartValid_WithValidItems_ShoulReturnTrue()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithStandardCart()
                .Build();

            // Act
            var result = arrangement.SUT.CartValid(arrangement.Cart);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CartValid_WithInValidItems_ShoulReturnFalse()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithStandardCart()
                .Build();

            arrangement.Cart.Items.Add("iPhone", 5);

            // Act
            var result = arrangement.SUT.CartValid(arrangement.Cart);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CartReturn_WithExistingCart_ShouldReturnCorrectCart()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(true)
                .Build();

            // Act
            var result = arrangement.SUT.CartReturn(arrangement.CartId);

            // Assert
            result.Should().BeOfType<CartReturnModel>();
            result.Items.Should().BeEquivalentTo(arrangement.ExistingCart.Items);
            result.TotalPrice.Should().Be(2500);
         }

        [Fact]
        public void CartReturn_WithNonExistantCart_ShouldReturnNull()
        {
            // Arrange
            var arrangement = new ArrangementBuilder()
                .WithExistingCart(false)
                .Build();

            // Act
            var result = arrangement.SUT.CartReturn(arrangement.CartId);

            // Assert
            result.Should().BeNull();
        }
    }
}
