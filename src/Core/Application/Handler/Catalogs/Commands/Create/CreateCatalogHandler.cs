using Application._Common;
using Application.Contracts;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Handler.Catalogs.Commands.CreateCatalog
{
    public class CreateCatalogHandler(ICatalogRepository repository) : IUseCase<CreateCatalogCommand, MessageResponseDto<object>>
    {
        public async Task<MessageResponseDto<object>> Handle(CreateCatalogCommand input, CancellationToken ct)
        {
            var existingCatalog = await repository.CatalogExistsAsync(input.Name, ct);
            if (existingCatalog)
            {
                return new MessageResponseDto<object>(false, $"Catalog with name {input.Name} already exists.");
            }
            var catalog = new Catalog(
                Guid.NewGuid(),
                input.Name,
                input.Description
            );
            await repository.AddCatalogAsync(catalog, ct);
            return new MessageResponseDto<object>(true, "Catalog created successfully");
        }
    }
}
