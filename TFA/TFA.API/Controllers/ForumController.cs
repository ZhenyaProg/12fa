using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFA.Storage;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetForums([FromServices] ForumDbContext dbContext, CancellationToken token)
        {
            var forumTitles =  await dbContext.Forums.Select(f => f.Title).ToArrayAsync(token);
            return Ok(forumTitles);
        }
    }
}