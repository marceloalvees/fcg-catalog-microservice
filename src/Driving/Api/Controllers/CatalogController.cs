using Application.Handler.Catalogs.Commands.AddGameToCatalog;
using Application.Handler.Catalogs.Commands.CreateCatalog;
using Application.Handler.Catalogs.Commands.Delete;
using Application.Handler.Catalogs.Commands.RemoveGameFromCatalog;
using Application.Handler.Catalogs.Commands.Update;
using Application.Handler.Catalogs.Queries.GetCatalogByKey;
using Application.Handler.Catalogs.Queries.GetCatalogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {

        [HttpGet()]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var request = new GetCatalogsQuery();
            var result = await mediator.Send(new GetCatalogsQuery(),cancellationToken);
            return Ok(result);
        }

        [HttpGet("{key:guid}")]
        public async Task<IActionResult> GetById(Guid key, CancellationToken cancellationToken)
        {
            var request = new GetCatalogByKeyQuery(key);
            var result = await mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateCatalogCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{key:guid}")]
        public async Task<IActionResult> Update(Guid key, UpdateCatalogCommand command, CancellationToken cancellationToken)
        {
            command.Key = key;
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{key:guid}")]
        public async Task<IActionResult> Delete(Guid key, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteCatalogCommand(key), cancellationToken);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddGameToCatalog(AddGameToCatalogCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveGameFromCatalog(RemoveGameFromCatalogCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    };
}
