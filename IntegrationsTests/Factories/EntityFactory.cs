using Domain.Entities;

namespace IntegrationTests.Factories
{
    public class EntityFactory
    {
        public Catalog Catalog => new(
            Guid.NewGuid(),
            "Sample Catalog",
            "This is a sample catalog for testing purposes."
        );
    }
}
