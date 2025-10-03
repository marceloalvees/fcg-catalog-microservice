using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Commands.CreateCatalog
{
    public class CreateCatalogCommand : IUseCaseInput<MessageResponseDto<object>>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
