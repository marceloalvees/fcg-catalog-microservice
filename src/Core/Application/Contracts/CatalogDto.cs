using Domain.Entities;

namespace Application.Contracts
{
    public class CatalogDto
    {
        public Guid? Key { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<GameDto> Games { get; set; } = new();


        public List<CatalogDto> ToCatalogDto(Catalog catalog)
        {
            return new List<CatalogDto>
            {
                new CatalogDto
                {
                    Key = catalog.Key,
                    Name = catalog.Name,
                    Description = catalog.Description,
                    Games = catalog.Games?.Select(g => new GameDto
                    {
                        Key = g.Key,
                        Title = g.Title,
                        Description = g.Description
                    }).ToList() ?? new List<GameDto>()
                }
            };
        }
    }
}
