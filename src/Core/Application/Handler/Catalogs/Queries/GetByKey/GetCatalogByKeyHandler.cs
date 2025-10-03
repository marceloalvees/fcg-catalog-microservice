using Application._Common;
using Application.Contracts;
using Domain.Abstractions;

namespace Application.Handler.Catalogs.Queries.GetCatalogByKey
{
    public class GetCatalogByKeyHandler(ICatalogRepository repository) : IUseCase<GetCatalogByKeyQuery, MessageResponseDto<CatalogDto>>
    {
        public async Task<MessageResponseDto<CatalogDto>> Handle(GetCatalogByKeyQuery input, CancellationToken ct)
        {
            var response =  await repository.GetCatalogByKeyAsync(input.key, ct);
            if (response == null)
            {
                return new MessageResponseDto<CatalogDto>(false, "Catalog not found", new CatalogDto());
            }
            var catalogDto = MapToDto(new List<Domain.Entities.Catalog> { response }).FirstOrDefault();
            return new MessageResponseDto<CatalogDto>(true, "", catalogDto);
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
                    Description = g.Description,
                    CatalogKey = g.CatalogKey

                }).ToList()
            }).ToList();
        }
        #endregion  
    }
}
