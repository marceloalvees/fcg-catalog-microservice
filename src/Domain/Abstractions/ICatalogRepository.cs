using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ICatalogRepository
    {
        Task<List<Catalog>> GetCatalogsAsync(CancellationToken cancellationToken);
        Task<Catalog?> GetCatalogByKeyAsync(Guid key, CancellationToken cancellationToken);
        Task<bool> CatalogExistsAsync(string name, CancellationToken cancellationToken);
        Task AddCatalogAsync(Catalog catalog, CancellationToken cancellationToken);
        Task UpdateCatalogAsync(Catalog catalog, CancellationToken cancellationToken);
        Task DeleteCatalogAsync(Guid key, CancellationToken cancellationToken);
    }
}
