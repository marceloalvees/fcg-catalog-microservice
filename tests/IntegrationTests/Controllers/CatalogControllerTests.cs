using Domain.Abstractions;

namespace IntegrationTests.Controllers
{
    [TestFixture]
    public class CatalogControllerTests
    {
        private FcgFixture _fixture;
        private ICatalogRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _fixture = new FcgFixture();
            Initialize(_fixture, "catalog");
            
        }
    }
}
