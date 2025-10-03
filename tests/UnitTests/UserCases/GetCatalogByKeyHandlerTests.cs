using Application.Handler.Catalogs.Queries.GetCatalogByKey;
using Domain.Abstractions;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class GetCatalogByKeyHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private GetCatalogByKeyHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new GetCatalogByKeyHandler(_repositoryMock.Object);
        }
        [Test]
        public async Task Handle_ShouldReturnCatalog_WhenCatalogExists()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var query = new GetCatalogByKeyQuery(catalogKey);
            var catalog = new Domain.Entities.Catalog(catalogKey, "Test Catalog", "A test catalog");
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            // Act
            var response = await _handler.Handle(query, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("");
            response.Data.Should().NotBeNull();
            response.Data.Key.Should().Be(catalogKey);
            response.Data.Name.Should().Be("Test Catalog");
        }
        [Test]
        public async Task Handle_ShouldReturnNotFound_WhenCatalogDoesNotExist()
        {
            // Arrange
            var query = new GetCatalogByKeyQuery(Guid.NewGuid());
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(query.key, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Domain.Entities.Catalog?)null);
            // Act
            var response = await _handler.Handle(query, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Catalog not found");
        }
    }
}
