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
            var key = Guid.NewGuid();
            var randomName = $"Catalog_{Guid.NewGuid()}";
            // Arrange
            var catalog = new Catalog(key, randomName, "A description for the new catalog");

            // Act
            var (httpMessage, response) = await Requester.PostAsync<MessageResponseDto<object>>(Uri, catalog, CancellationToken);

            // Assert
            httpMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Catalog created successfully");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCatalogs()
        {
            // Act
            var (httpMessage, response) = await Requester.GetAsync<MessageResponseDto<List<Catalog>>>(Uri, CancellationToken);
            // Assert
            httpMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByKey_ShouldReturnCatalog_WhenCatalogExists()
        {
            // Arrange
            var key = Guid.NewGuid();
            var randomName = $"Catalog_{Guid.NewGuid()}";

            var catalog = new Catalog(key, randomName, "A description for the new catalog");
            await _repository.AddCatalogAsync(catalog, CancellationToken);
            var fullUri = new Uri($"{Uri}/{key}");
            // Act
            var (httpMessage, response) = await Requester.GetAsync<MessageResponseDto<Catalog>>(fullUri, CancellationToken);
            // Assert
            httpMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
        }
    }
}
