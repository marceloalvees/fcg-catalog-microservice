using Application.Handler.Catalogs.Commands.CreateCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
