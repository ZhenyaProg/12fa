using Microsoft.AspNetCore.Mvc;
using TFA.API.Contracts;
using TFA.Application.Authorization;
using TFA.Application.Exceptions;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(ForumResponse[]))]
        public async Task<IActionResult> GetForums([FromServices] IGetForumsUseCase useCase,
                                                   CancellationToken token)
        {
            var forums = await useCase.Execute(token);
            return Ok(forums.Select(f => new ForumResponse
            {
                Id = f.Id,
                Title = f.Title,
            }));
        }

        [HttpPost("{forumId:guid}/topics")]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(TopicResponse))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopicRequest request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken token)
        {
            try
            {
                var topic = await useCase.Execute(forumId, request.Title, token);

                return CreatedAtRoute(nameof(GetForums), new TopicResponse
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    CreatedDate = topic.CreatedDate,
                });
            }
            catch(IntentionManagerException ex)
            {
                return Forbid();
            }
            catch (ForumNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}