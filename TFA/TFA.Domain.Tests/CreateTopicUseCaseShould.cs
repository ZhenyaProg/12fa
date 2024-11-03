using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TFA.Application;
using TFA.Application.Exceptions;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;
using TFA.Storage;
using Moq.Language.Flow;

namespace TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _createTopic;
        private readonly ForumDbContext _forumDbContext;
        private readonly ISetup<IGuidFactory, Guid> _createIdSetup;
        private readonly ISetup<IMomentProvider, DateTimeOffset> _momentProviderSetup;

        public CreateTopicUseCaseShould()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
            _forumDbContext = new ForumDbContext(optionsBuilder.Options);

            var guidFactory = new Mock<IGuidFactory>();
            _createIdSetup = guidFactory.Setup(f => f.Create());

            var momentProvider = new Mock<IMomentProvider>();
            _momentProviderSetup = momentProvider.Setup(m => m.Now);

            _createTopic = new CreateTopicUseCase(guidFactory.Object, momentProvider.Object, _forumDbContext);
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            await _forumDbContext.Forums.AddAsync(new ForumEntity
            {
                Id = Guid.Parse("e6d6b7eb-13df-4f7c-b6d8-89fe88bfe727"),
                Title = "Forum Title"
            });
            await _forumDbContext.SaveChangesAsync();

            var forumId = Guid.Parse("14ef39d4-44d4-4660-9c90-b836f53c40cb");
            var authorId = Guid.Parse("f1fae336-71a4-46dc-8cf9-e0357eb8a2b5");

            await _createTopic
                    .Invoking(s => s.Execute(forumId, "Forum Title", authorId, CancellationToken.None))
                    .Should().ThrowAsync<ForumNotFoundException>();
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic()
        {
            Guid forumId = Guid.Parse("3ca4604c-c046-4ded-9b39-337d1a45dda2");
            Guid userId = Guid.Parse("9298f1de-e294-4fb6-a2f6-d3abd29a1d08");
            await _forumDbContext.Forums.AddAsync(new ForumEntity
            {
                Id = forumId,
                Title = "Existing forum"
            });
            await _forumDbContext.Users.AddAsync(new User
            {
                Id = userId,
                Login = "no_zhenya"
            });
            await _forumDbContext.SaveChangesAsync();

            _createIdSetup.Returns(Guid.Parse("e7f6363d-f291-4a81-ae99-05ef0252949b"));
            _momentProviderSetup.Returns(new DateTimeOffset(2024, 11, 2, 20, 23, 23, TimeSpan.FromHours(3)));

            Topic actual = await _createTopic.Execute(forumId, "Forum 1", userId, CancellationToken.None);

            var topics = await _forumDbContext.Topics.ToArrayAsync();
            topics.Should().BeEquivalentTo(new[]
            {
                new TopicEntity
                {
                    AuthorId = userId,
                    ForumId = forumId,
                    Title = "Forum 1",
                },
            }, cfg => cfg.Including(t => t.Title)
                         .Including(t => t.AuthorId)
                         .Including(t => t.ForumId));
            actual.Should().BeEquivalentTo(new Topic
            {
                Id = Guid.Parse("e7f6363d-f291-4a81-ae99-05ef0252949b"),
                Title = "Forum 1",
                Author = "no_zhenya",
                CreatedDate = new DateTimeOffset(2024, 11, 2, 20, 23, 23, TimeSpan.FromHours(3)),
            });
        }
    }
}