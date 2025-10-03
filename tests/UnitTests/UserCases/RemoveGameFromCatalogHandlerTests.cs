using Application.Handler.Catalogs.Commands.RemoveGameFromCatalog;
using Domain.Abstractions;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class RemoveGameFromCatalogHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private RemoveGameFromCatalogHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new RemoveGameFromCatalogHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldRemoveGameFromCatalog_WhenCatalogAndGameExist()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var gameKey = Guid.NewGuid();
            var command = new RemoveGameFromCatalogCommand
            {
                CatalogKey = catalogKey,
                Key = gameKey,
                Description = "A description for the game to be removed",
                Title = "Game to be removed"
            };
            var catalog = new Catalog(catalogKey, "Test Catalog", "A test catalog");
            var game = new Game(gameKey, "Test Game", "A test game", catalogKey);
            catalog.AddGame(game);
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            _repositoryMock.Setup(r => r.UpdateCatalogAsync(It.IsAny<Catalog>(), It.IsAny<CancellationToken>()));
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Game removed from catalog successfully");
            catalog.Games.Should().NotContain(g => g.Key == gameKey);
        }

        [Test]
        public async Task Handle_ShouldNotRemoveGameFromCatalog_WhenCatalogDoesNotExist()
        {
            // Arrange
            var command = new RemoveGameFromCatalogCommand
            {
                CatalogKey = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Description = "A description for the game to be removed",
                Title = "Game to be removed"
            };
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.CatalogKey.GetValueOrDefault(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Catalog?)null);
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Catalog with key {command.CatalogKey} does not exist.");
        }

        [Test]
        public async Task Handle_ShouldNotRemoveGameFromCatalog_WhenGameDoesNotExist()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var command = new RemoveGameFromCatalogCommand
            {
                CatalogKey = catalogKey,
                Key = Guid.NewGuid(),
                Description = "A description for the game to be removed",
                Title = "Game to be removed"
            };
            var catalog = new Catalog(catalogKey, "Test Catalog", "A test catalog");
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Game with key {command.Key} does not exist in the catalog.");
            catalog.Games.Should().BeEmpty();
        }
    }
}
