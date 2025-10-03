using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Commands.Delete
{
    public record DeleteCatalogCommand(Guid Key) : IUseCaseInput<MessageResponseDto<object>>;
}
