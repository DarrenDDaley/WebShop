using FluentAssertions;
using Items.API.Controllers;
using Items.API.Managers;
using System;
using Xunit;

namespace Items.API.Tests
{
    public class ItemsControllerTests
    {
        [Fact]
        public void Ctor_WithNullEndpoint_ShouldThrowException()
        {
            // Act 
            var error = Record.Exception(() => new ItemsController(new CatalogManager(), null));

            // Arrange

            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: endpoint");
        }

        [Fact]
        public void Ctor_WithNullCatalogManager_ShouldThrowException()
        {
            // Arrange
            var endpoint = new EndpointStub();

            // Act 
            var error = Record.Exception(() => new ItemsController(null, endpoint));

            // Arrange

            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: catalogManager");
        }

    }
}
