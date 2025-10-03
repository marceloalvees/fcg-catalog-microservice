using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Commands.AddGameToCatalog
{
    public class AddGameToCatalogCommand : GameDto, IUseCaseInput<MessageResponseDto<object>>
    {

    }
}
