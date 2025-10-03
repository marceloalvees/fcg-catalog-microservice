using Application.Contracts;

namespace Application.Handler.Games.Queries.GetGames
{
    public record GetGamesResponse(List<GameDto> Games);
}
