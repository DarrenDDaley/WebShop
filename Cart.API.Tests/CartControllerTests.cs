using Cart.API.Controllers;
using Cart.API.Managers;
using Cart.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cart.API.Tests
{
    public class CartControllerTests
    {
        private class Arragement
        {
            public Guid CartId { get; } = Guid.NewGuid();

            public CartController SUT { get; }

            public CartApiModel CartModel { get; } = new CartApiModel();

            public Arragement(ICartManager cartManager)
            {
                IEndpointInstance endpoint = new EndpointStub();

                SUT = new CartController(cartManager, endpoint);
            }
        }

        private class ArrangementsBuilder
        {
            CartManagerStub cartManager = new CartManagerStub();

            public ArrangementsBuilder WithExistingCart(bool cartExists)
            {
                cartManager.CartExists = cartExists;
                return this;
            }

            public ArrangementsBuilder WithValidCart(bool validCart)
            {
                cartManager.ValidCart = validCart;
                return this;
            }

            public ArrangementsBuilder WithItemsAddedToCart(bool addedtoCart)
            {
                cartManager.AddItemCartExists = addedtoCart;
                return this;
            }

            public ArrangementsBuilder WithItemsRemovedFomCart(bool removetoCart)
            {
                cartManager.RemoveCartExists = removetoCart;
                return this;
            }

            public Arragement Build() {
                return new Arragement(cartManager);
            }
        }

        [Fact]
        public void Ctor_WithNullCartManager_ShouldThrowException()
        {
            // Act 
            var error = Record.Exception(() => new CartController(null, new EndpointStub()));

            // Assert

            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: cartManager");
        }

        [Fact]
        public void Ctor_WithNullEndpoint_ShouldThrowException()
        {
            // Act 
            var error = Record.Exception(() => new CartController(new CartManagerStub(), null));

            // Assert

            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: endpoint");
        }

        [Fact]
        public void Get_WithCartExisting_ShouldReturnModel()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithExistingCart(true)
                .Build();

            // Act 
            var result = arrangement.SUT.Get(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            
            var resultObject = result as OkObjectResult;
            resultObject.Value.Should().BeOfType<CartReturnModel>();
        }

        [Fact]
        public void Get_WithoutCartExisting_ShouldReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithExistingCart(false)
                .Build();

            // Act 
            var result = arrangement.SUT.Get(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("The cart specified doesn't exist.");
        }

        [Fact]
        public void CartApiModelValidation_Null_ShouldValidationError()
        {
            // Arrange
            var arrangement = new CartApiModel() { Items = null };

            // Act 
            var validationContext = new ValidationContext(arrangement);

            var result = arrangement.Validate(validationContext);

            // Assert
            result.Count().Should().Be(1);
            result.First().ErrorMessage.Should().Be("The cart cant be empty.");
        }

        [Fact]
        public async Task CreateCart_WithValidModel_ShoulReturnId()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(true)
                .Build();

            // Act 
            var result = await arrangement.SUT.CreateCartAsync(arrangement.CartModel);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var resultObject = result as OkObjectResult;
            resultObject.Value.Should().BeOfType<Guid>();
        }

        [Fact]
        public async Task CreateCart_WithInValidModel_ShoulReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(false)
                .Build();

            // Act 
            var result = await arrangement.SUT.CreateCartAsync(arrangement.CartModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("Your cart has an invalid item.");
        }

        [Fact]
        public async Task AddToCart_ValidItems_ShoulReturnOk()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(true)
                .WithItemsAddedToCart(true)
                .Build();

            // Act 
            var result = await arrangement.SUT
                .AddToCartAsync(arrangement.CartId, arrangement.CartModel);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AddToCart_WithInValidCart_ShoulReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(false)
                .Build();

            // Act 
            var result = await arrangement.SUT
                .AddToCartAsync(arrangement.CartId, arrangement.CartModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("Your cart has an invalid item.");
        }

        [Fact]
        public async Task AddToCart_ItemsDontExistInCart_ShoulReturnBadRequest()
        {
            // Arrange
            var arrnagement = new ArrangementsBuilder()
                .WithValidCart(true)
                .WithItemsAddedToCart(false)
                .Build();

            // Act 
            var result = await arrnagement.SUT
                .AddToCartAsync(arrnagement.CartId, arrnagement.CartModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("The cart items specified dont exist.");
        }

        [Fact]
        public async Task RemoveToCart_ValidItems_ShoulReturnOk()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(true)
                .WithItemsRemovedFomCart(true)
                .Build();

            // Act 
            var result = await arrangement.SUT
                .RemoveFromCartAsync(arrangement.CartId, arrangement.CartModel);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RemoveToCart_WithInValidCart_ShoulReturnBadRequest()
        {
            // Arrange
            var arrangement = new ArrangementsBuilder()
                .WithValidCart(false)
                .Build();

            // Act 
            var result = await arrangement.SUT
                .RemoveFromCartAsync(arrangement.CartId, arrangement.CartModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("Your cart has an invalid item.");
        }

        [Fact]
        public async Task RemoveToCart_ItemsDontExistInCart_ShoulReturnBadRequest()
        {
            // Arrange
            var arrnagement = new ArrangementsBuilder()
                .WithValidCart(true)
                .WithItemsRemovedFomCart(false)
                .Build();

            // Act 
            var result = await arrnagement.SUT
                .RemoveFromCartAsync(arrnagement.CartId, arrnagement.CartModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

            var resultObject = result as BadRequestObjectResult;
            resultObject.Value.Should().Be("The cart items specified dont exist.");
        }
    }
}
