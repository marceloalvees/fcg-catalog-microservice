using Application.Handler.Games.Queries.GetGames;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController(IMediator mediator) : Controller
    {



    }
}
