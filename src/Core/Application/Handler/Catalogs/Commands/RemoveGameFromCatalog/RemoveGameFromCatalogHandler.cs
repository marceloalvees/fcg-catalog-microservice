using Application._Common;
using Application.Contracts;
using Domain.Abstractions;

namespace Application.Handler.Catalogs.Commands.RemoveGameFromCatalog
{
    public class RemoveGameFromCatalogHandler(ICatalogRepository repository) : IUseCase<RemoveGameFromCatalogCommand, MessageResponseDto<object>>
    {
        public async Task<MessageResponseDto<object>> Handle(RemoveGameFromCatalogCommand input, CancellationToken ct)
        {
            var catalog = await repository.GetCatalogByKeyAsync(input.CatalogKey.GetValueOrDefault(), ct);
            if (catalog == null)
            {
                return new MessageResponseDto<object>(false, $"Catalog with key {input.CatalogKey} does not exist.");

            }
            var game = catalog.Games.FirstOrDefault(g => g.Key == input.Key);
            catalog.RemoveGame(game);
            await repository.UpdateCatalogAsync(catalog, ct);
            return new MessageResponseDto<object>(true, "Game removed from catalog successfully");


        }
    }
}
