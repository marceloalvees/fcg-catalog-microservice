using Application._Common;
using Application.Contracts;
using Domain.Abstractions;

namespace Application.Handler.Catalogs.Commands.Update
{
    public class UpdateCatalogHandler(ICatalogRepository repository) : IUseCase<UpdateCatalogCommand, MessageResponseDto<object>>
    {
        public async Task<MessageResponseDto<object>> Handle(UpdateCatalogCommand input, CancellationToken ct)
        {
            var existingCatalog = await repository.GetCatalogByKeyAsync(input.Key.GetValueOrDefault(), ct);
            if (existingCatalog == null)
            {
                throw new Exception($"Catalog with key {input.Key} does not exist.");
            }
            existingCatalog.SetData(input.Name, input.Description);

            await repository.UpdateCatalogAsync(existingCatalog, ct);
            return new MessageResponseDto<object>(true, "catalog updated successfully");
        }
    }
}
