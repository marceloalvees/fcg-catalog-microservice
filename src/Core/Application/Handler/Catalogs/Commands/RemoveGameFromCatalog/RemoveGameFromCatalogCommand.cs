using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Commands.RemoveGameFromCatalog
{
    public class RemoveGameFromCatalogCommand : GameDto, IUseCaseInput<MessageResponseDto<object>>;
}
