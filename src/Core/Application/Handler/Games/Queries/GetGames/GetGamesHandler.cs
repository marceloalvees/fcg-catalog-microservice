using Application._Common;
using Application.Contracts;
using Domain.Abstractions;

namespace Application.Handler.Games.Queries.GetGames
{
    public class GetGamesHandler(IGameRepository repository) : IUseCase<GetGamesQuery, GetGamesResponse>
    {
        public Task<GetGamesResponse> Handle(GetGamesQuery input, CancellationToken ct)
        {
            var games = repository.GetGamesAsync(ct);
            var gameDtos = games.Result.Select(g => new GameDto
            {
                Key = g.Key,
                Title = g.Title,
                Description = g.Description
            }).ToList();
            return Task.FromResult(new GetGamesResponse(gameDtos));
        }
    }
}
