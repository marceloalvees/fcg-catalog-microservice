using Application.Handler.Catalogs.Commands.Delete;
using Domain.Abstractions;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class DeleteCatalogHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private DeleteCatalogHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new DeleteCatalogHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteCatalog_WhenCatalogExists()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var command = new DeleteCatalogCommand(catalogKey);
            var catalog = new Domain.Entities.Catalog(catalogKey, "Test Catalog", "A test catalog");
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            _repositoryMock.Setup(r => r.DeleteCatalogAsync(catalogKey, It.IsAny<CancellationToken>()));
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Catalog deleted successfully");
        }


        [Test]
        public async Task Handle_ShouldNotDeleteCatalog_WhenCatalogDoesNotExist()
        {
            // Arrange
            var command = new DeleteCatalogCommand(Guid.NewGuid());
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
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
            var command = new DeleteCatalogCommand(Guid.NewGuid());
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new Exception("Database error"));
            // Act & Assert
            Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }
    }
}
