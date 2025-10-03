using Amazon.Auth.AccessControlPolicy;
using Domain.Abstractions;
using Domain.Entities;
using OpenSearch.Client;

namespace Infrastructure.ElasticSerach.Repository
{
    public class CatalogRepository(IOpenSearchClient elasticClient) : ICatalogRepository
    { 
        public async Task<List<Catalog>> GetCatalogsAsync(CancellationToken cancellationToken)
        {
            var searchResponse = await elasticClient.SearchAsync<Catalog>(s => s
                .Index("catalogs")
                .Query(q => q.MatchAll()), cancellationToken);
            return searchResponse.Documents.ToList();
        }

        public async Task<Catalog?> GetCatalogByKeyAsync(Guid key, CancellationToken cancellationToken)
        {
            var response = await elasticClient.GetAsync<Catalog>(key.ToString(), g => g.Index("catalogs"), cancellationToken);
            EnsureValidResponse<Catalog>(response, $"Failed to retrieve catalog with ID {key}");
            return response.Found ? response.Source : null;
        }

        public async Task<bool> CatalogExistsAsync(string name, CancellationToken cancellationToken)
        {
            var searchResponse = await elasticClient.SearchAsync<Catalog>(s => s
                .Index("catalogs")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(name))), cancellationToken);
            return searchResponse.Documents.Any();
        }

        public async Task AddCatalogAsync(Catalog catalog, CancellationToken cancellationToken)
        {
            var response = await elasticClient.IndexAsync(catalog, i => i.Index("catalogs").Id(catalog.Key), cancellationToken);
            EnsureValidResponse<Catalog>(response, $"Failed to add catalog with ID {catalog.Key}");
        }
        public async Task UpdateCatalogAsync(Catalog catalog, CancellationToken cancellationToken)
        {
            var response = await elasticClient.IndexAsync(catalog, i => i.Index("catalogs").Id(catalog.Key), cancellationToken);
            EnsureValidResponse<Catalog>(response, $"Failed to update catalog with ID {catalog.Key}");
        }
        public async Task DeleteCatalogAsync(Guid key, CancellationToken cancellationToken)
        {
            var response = await elasticClient.DeleteAsync<Catalog>(key.ToString(), d => d.Index("catalogs"), cancellationToken);
            EnsureValidResponse<Catalog>(response, $"Failed to delete catalog with ID {key}");
        }

        private static void EnsureValidResponse<T>(IResponse response, string errorMessage)
        {
            if (!response.IsValid)
            {
                throw new Exception($"{errorMessage}: {response.ServerError?.Error.Reason}");
            }
        }
    }
}
