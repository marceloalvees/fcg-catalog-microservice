using Application._Common;
using Application.Contracts;

namespace Application.Handler.Catalogs.Queries.GetCatalogs
{
    public record class GetCatalogsQuery() : IUseCaseInput<MessageResponseDto<List<CatalogDto>>>;
}
