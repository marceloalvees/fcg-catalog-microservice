using Application._Common;
using Application.Contracts;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Handler.Catalogs.Commands.AddGameToCatalog
{
    public class AddGameToCatalogHandler(ICatalogRepository repository) : IUseCase<AddGameToCatalogCommand, MessageResponseDto<object>>
    {
        public async Task<MessageResponseDto<object>> Handle(AddGameToCatalogCommand input, CancellationToken ct)
        {
            var catalog = await repository.GetCatalogByKeyAsync(input.CatalogKey.GetValueOrDefault(), ct);
            var game = new Game(input.Key.GetValueOrDefault(),input.Title, input.Description, input.CatalogKey.GetValueOrDefault());
            if (catalog == null)
            {
                return new MessageResponseDto<object>(false, $"Catalog with key {input.CatalogKey} does not exist.");

            }
            catalog.AddGame(game);
            await repository.UpdateCatalogAsync(catalog, ct);
            return new MessageResponseDto<object>(true, "Game added to catalog successfully");
        }
    }
}
