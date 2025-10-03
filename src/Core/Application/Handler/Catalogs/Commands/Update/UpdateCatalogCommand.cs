using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Commands.Update
{
    public class UpdateCatalogCommand  : IUseCaseInput<MessageResponseDto<object>>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid? Key { get; set; }
    }
}
