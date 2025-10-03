using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IGameRepository
    {
        Task<List<Game>> GetGamesAsync(CancellationToken cancellationToken);
    }
}
