using Application.Handler.Catalogs.Commands.AddGameToCatalog;
using Domain.Abstractions;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class AddGameToCatalogHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private AddGameToCatalogHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new AddGameToCatalogHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldAddGameToCatalog_WhenCatalogExists()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var gameKey = Guid.NewGuid();
            var command = new AddGameToCatalogCommand
            {
                CatalogKey = catalogKey,
                Key = gameKey,
                Title = "New Game",
                Description = "A description for the new game"
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
            response.Message.Should().Be("Game added to catalog successfully");
            catalog.Games.Should().ContainSingle(g => g.Key == gameKey && g.Title == "New Game");
        }

        [Test]
        public async Task Handle_ShouldNotAddGameToCatalog_WhenCatalogDoesNotExist()
        {
            // Arrange
            var command = new AddGameToCatalogCommand
            {
                CatalogKey = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Title = "New Game",
                Description = "A description for the new game"
            };
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(command.CatalogKey.GetValueOrDefault(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Domain.Entities.Catalog?)null);
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Catalog with key {command.CatalogKey} does not exist.");
        }

        [Test]
        public async Task Handle_ShouldNotAddGameToCatalog_WhenGameAlreadyExistsInCatalog()
        {
            // Arrange
            var catalogKey = Guid.NewGuid();
            var gameKey = Guid.NewGuid();
            var command = new AddGameToCatalogCommand
            {
                CatalogKey = catalogKey,
                Key = gameKey,
                Title = "Existing Game",
                Description = "A description for the existing game"
            };
            var catalog = new Domain.Entities.Catalog(catalogKey, "Test Catalog", "A test catalog");
            var existingGame = new Domain.Entities.Game(gameKey, "Existing Game", "A description for the existing game", catalogKey);
            catalog.AddGame(existingGame);
            _repositoryMock.Setup(r => r.GetCatalogByKeyAsync(catalogKey, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalog);
            // Act
            Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
            // Assert
            await act.Should().ThrowAsync<Domain._Common.Exceptions.FCGDuplicateException>()
                     .WithMessage($"*{gameKey}*");
        }
    }
}

