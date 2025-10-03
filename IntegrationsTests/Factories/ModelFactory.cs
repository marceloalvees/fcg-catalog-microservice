using Application.Handler.Catalogs.Commands.CreateCatalog;

namespace IntegrationTests.Factories
{
    public class ModelFactory
    {
        public CreateCatalogCommand CreateCatalogCommand => new CreateCatalogCommand
        {
            Name = "Test Catalog",
            Description = "A test catalog description"
        };
    }
}
