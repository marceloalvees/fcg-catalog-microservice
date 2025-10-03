using Application.Handler.Catalogs.Commands.Update;
using Domain.Abstractions;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class UpdateCatalogHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private UpdateCatalogHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new UpdateCatalogHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdateCatalog_WhenCatalogExists()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var command = new UpdateCatalogCommand
            {
                Key = catalogKey,
                Name = "Updated Catalog",
                Description = "An updated description for the catalog"
            };
            var catalog = new Domain.Entities.Catalog(catalogKey, "Test Catalog", "A test catalog");
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            _repositoryMock.Setup(r => r.UpdateCatalogAsync(It.IsAny<Domain.Entities.Catalog>(), It.IsAny<CancellationToken>()));
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Catalog updated successfully");
            catalog.Name.Should().Be("Updated Catalog");
            catalog.Description.Should().Be("An updated description for the catalog");
        }

        [Test]
        public async Task Handle_ShouldNotUpdateCatalog_WhenCatalogDoesNotExist()
        {
            // Arrange
            var command = new UpdateCatalogCommand
            {
                Key = Guid.NewGuid(),
                Name = "Updated Catalog",
                Description = "An updated description for the catalog"
            };
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.Key.GetValueOrDefault(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Domain.Entities.Catalog?)null);
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Catalog with key {command.Key} does not exist.");
        }

        [Test]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var command = new UpdateCatalogCommand
            {
                Key = Guid.NewGuid(),
                Name = "Updated Catalog",
                Description = "An updated description for the catalog"
            };
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.Key.GetValueOrDefault(), It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new Exception("Database error"));
            // Act
            Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }
    }
}
