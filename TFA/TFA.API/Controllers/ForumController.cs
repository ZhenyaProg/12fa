using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TFA.API.Contracts;
using TFA.Application.UseCases.CreateForum;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;
using TFA.Application.UseCases.GetTopics;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        [HttpPost(Name = nameof(CreateForum))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(201, Type = typeof(ForumResponse))]
        public async Task<IActionResult> CreateForum(
            [FromBody] CreateForumRequest requet,
            [FromServices] ICreateForumUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var command = new CreateForumCommand(requet.Title);
            var forum = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(CreateForum), mapper.Map<ForumResponse>(forum));
        }

        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(ForumResponse[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken token)
        {
            var forums = await useCase.Execute(token);
            return Ok(forums.Select(mapper.Map<ForumResponse>));
        }

        [HttpPost("{forumId:guid}/topics", Name = nameof(CreateTopic))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(TopicResponse))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopicRequest request,
            [FromServices] ICreateTopicUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken token)
        {
            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, token);

            return CreatedAtRoute(nameof(CreateTopic), mapper.Map<TopicResponse>(topic));
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
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            GetTopicsQuery query = new GetTopicsQuery(forumId, skip, take);
            var (resources, totalCount) = await useCase.Execute(query, cancellationToken);
            return Ok(new { resources = resources.Select(mapper.Map<TopicResponse>), totalCount });
        }
    }
}