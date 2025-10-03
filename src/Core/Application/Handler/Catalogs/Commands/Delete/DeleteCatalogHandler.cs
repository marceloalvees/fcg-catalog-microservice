using Application._Common;
using Application.Contracts;
using Domain.Abstractions;
namespace Application.Handler.Catalogs.Commands.Delete
{
    public class DeleteCatalogHandler(ICatalogRepository repository) : IUseCase<DeleteCatalogCommand, MessageResponseDto<object>>
    {
        public async Task<MessageResponseDto<object>> Handle(DeleteCatalogCommand input, CancellationToken ct)
        {
            var existingCatalog = await repository.GetCatalogByKeyAsync(input.Key, ct);
            if (existingCatalog == null)
            {
                return new MessageResponseDto<object>(false, $"Catalog with key {input.Key} does not exist.");
            }
            await repository.DeleteCatalogAsync(input.Key, ct);
            return new MessageResponseDto<object>(true, "Catalog deleted successfully");
        }
    }
}
