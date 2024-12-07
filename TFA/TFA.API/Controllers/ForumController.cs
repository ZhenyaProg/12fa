using Microsoft.AspNetCore.Mvc;
using TFA.API.Contracts;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;
using TFA.Application.UseCases.GetTopics;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(ForumResponse[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
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
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(TopicResponse))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopicRequest request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken token)
        {
            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, token);

            return CreatedAtRoute(nameof(GetForums), new TopicResponse
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedDate = topic.CreatedDate,
            });
        }

        [HttpGet("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(410)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTopics(
            [FromRoute] Guid forumId,
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IGetTopicsUseCase useCase,
            CancellationToken cancellationToken)
        {
            GetTopicsQuery query = new GetTopicsQuery(forumId, skip, take);
            var (resources, totalCount) = await useCase.Execute(query, cancellationToken);
            return Ok(new { resources = resources.Select(r => new Topic
            {
                Id = r.Id,
                Title = r.Title,
                CreatedDate = r.CreatedDate,
            }), totalCount });
        }
    }
}