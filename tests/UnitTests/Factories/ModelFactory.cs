using Application.Handler.Catalogs.Commands.CreateCatalog;

namespace UnitTests.Factories
{
    public class ModelFactory
    {
        public CreateCatalogCommand CreateCatalogCommand =>
            new CreateCatalogCommand
            {
                Name = "Test Catalog",
                Description = "This is a test catalog"
            };
    }
}
