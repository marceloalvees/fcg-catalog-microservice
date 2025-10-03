using Application.Handler.Catalogs.Queries.GetCatalogs;
using Domain.Abstractions;
using FluentAssertions;
using Moq;

namespace UnitTests.UserCases
{
    [TestFixture]
    public class GetCatalogsHandlerTests
    {
        private Mock<ICatalogRepository> _repositoryMock;
        private GetCatalogsHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _handler = new GetCatalogsHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnCatalogs_WhenCatalogsExist()
        {
            // Arrange
            var catalogs = new List<Domain.Entities.Catalog>
            {
                new Domain.Entities.Catalog(Guid.NewGuid(), "Catalog 1", "Description 1"),
                new Domain.Entities.Catalog(Guid.NewGuid(), "Catalog 2", "Description 2")
            };
            _repositoryMock.Setup(r => r.GetCatalogsAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(catalogs);
            // Act
            var response = await _handler.Handle(new GetCatalogsQuery(), CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("");
            response.Data.Should().NotBeNull();
            response.Data.Count().Should().Be(2);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCatalogsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetCatalogsAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new List<Domain.Entities.Catalog>());
            // Act
            var response = await _handler.Handle(new GetCatalogsQuery(), CancellationToken.None);
            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("");
            response.Data.Should().NotBeNull();
            response.Data.Count().Should().Be(0);
        }

    }
}
