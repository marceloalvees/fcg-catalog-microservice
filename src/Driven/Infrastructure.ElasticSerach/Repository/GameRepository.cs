using Domain.Abstractions;
using Domain.Entities;
using OpenSearch.Client;

namespace Infrastructure.ElasticSerach.Repository
{
    public class GameRepository(IOpenSearchClient elasticClient) : IGameRepository
    {
     
        public async Task<List<Game>> GetGamesAsync(CancellationToken cancellationToken)
        {
            var searchResponse = await elasticClient.SearchAsync<Game>(s => s
                .Index("games")
                .Query(q => q.MatchAll()), cancellationToken);
            return searchResponse.Documents.ToList();
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
