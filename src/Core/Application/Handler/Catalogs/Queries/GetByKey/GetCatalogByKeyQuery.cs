using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Queries.GetCatalogByKey
{
    public record class GetCatalogByKeyQuery(Guid key) : IUseCaseInput<MessageResponseDto<CatalogDto>>;
}
