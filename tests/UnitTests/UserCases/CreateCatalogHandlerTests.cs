using Application.Handler.Catalogs.Commands.CreateCatalog;
using Domain.Abstractions;
using FluentAssertions;
using Moq;
using UnitTests._Common;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class CreateCatalogHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private CreateCatalogHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new CreateCatalogHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateCatalog_WhenCatalogExist()
        {
            // Arrange
            var command = new CreateCatalogCommand
            {
                Name = "New Catalog",
                Description = "A description for the new catalog"
            };
            _repositoryMock.Setup(r => r.CatalogExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(false);
            _repositoryMock.Setup(r => r.AddCatalogAsync(It.IsAny<Domain.Entities.Catalog>(), It.IsAny<CancellationToken>()));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Catalog created successfully");
        }

        [Test]
        public async Task Handle_ShouldNotCreateCatalog_WhenCatalogDoesNotExist()
        {
            // Arrange
            var command = new CreateCatalogCommand
            {
                Name = "Existing Catalog",
                Description = "A description for the existing catalog"
            };
            _repositoryMock.Setup(r => r.CatalogExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true);
            // Act
            var response = await _handler.Handle(command, CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Message.Should().Be($"Catalog with name {command.Name} already exists.");
        }
        [Test]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new CreateCatalogCommand
            {
                Name = "New Catalog",
                Description = "A description for the new catalog"
            };
            _repositoryMock.Setup(r => r.CatalogExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new Exception("Database error"));
            // Act
            Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }
    }
}
