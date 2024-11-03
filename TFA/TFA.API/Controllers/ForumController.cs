using Microsoft.AspNetCore.Mvc;
using TFA.Application.UseCases.GetForums;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Contracts.ForumResponse[]))]
        public async Task<IActionResult> GetForums([FromServices] IGetForumsUseCase useCase, CancellationToken token)
        {
            var forums = await useCase.Execute(token);
            return Ok(forums.Select(f => new Contracts.ForumResponse
            {
                Id = f.Id,
                Title = f.Title,
            }));
        }
    }
}