using Application.Contracts;
using Domain.Abstractions;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests._Common;
using System.Net;

namespace IntegrationTests.Controllers
{
    public class CatalogControllerTests : ControllerTestBase
    {
        private ICatalogRepository _repository;

        public CatalogControllerTests(FcgFixture fixture) : base(fixture, "catalog")
        {
            _repository = GetService<ICatalogRepository>();
        }
        [Fact]
        public async Task Create_ShouldCreateCatalog()
        {
            // Arrange
            var randomName = $"Catalog_{Guid.NewGuid()}";
            var catalog = new Catalog(Guid.NewGuid(), randomName, "A description for the new catalog");

            // Act
            var (httpMessage, response) = await Requester.PostAsync<MessageResponseDto<object>>(Uri, catalog, CancellationToken);

            // Assert
            httpMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Catalog created successfully");
        }
    }
}
