using Application._Common;
using Application.Contracts;
using Domain.Abstractions;

namespace Application.Handler.Catalogs.Queries.GetCatalogs
{
    public class GetCatalogsHandler(ICatalogRepository repository) : IUseCase<GetCatalogsQuery, MessageResponseDto<List<CatalogDto>>>
    {
        public async Task<MessageResponseDto<List<CatalogDto>>> Handle(GetCatalogsQuery input, CancellationToken ct)
        {
            var catalogs = await repository.GetCatalogsAsync(ct);
            var catalogDtos = MapToDto(catalogs);
            return new MessageResponseDto<List<CatalogDto>>(true, "", catalogDtos);
        }
        #region Private Methods
        private List<CatalogDto> MapToDto(List<Domain.Entities.Catalog> catalogs)
        {
            return catalogs.Select(c => new CatalogDto
            {
                Key = c.Key,
                Name = c.Name,
                Description = c.Description,
                Games = c.Games.Select(g => new GameDto
                {
                    Key = g.Key,
                    Title = g.Title,
                    Description = g.Description
                }).ToList()
            }).ToList();
        }
        #endregion
    }
}
