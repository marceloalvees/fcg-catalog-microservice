using Domain.Entities;

namespace Application.Contracts
{
    public class GameDto
    {
        public Guid? Key { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? CatalogKey { get; set; }
    }
}
