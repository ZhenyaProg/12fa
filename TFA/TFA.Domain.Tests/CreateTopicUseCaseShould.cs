using FluentAssertions;
using Moq;
using TFA.Application.Exceptions;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;
using Moq.Language.Flow;
using TFA.Application.Authentication;
using TFA.Application.Authorization;
using FluentValidation;
using FluentValidation.Results;

namespace TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _createTopic;
        private readonly Mock<ICreateTopicStorage> _storage;
        private readonly Mock<IIntentionManager> _intentionManager;
        private readonly ISetup<ICreateTopicStorage, Task<bool>> _forumExistsSetup;
        private readonly ISetup<ICreateTopicStorage, Task<Topic>> _createTopicSetup;
        private readonly ISetup<IIdentity, Guid> _getUserIdSetup;
        private readonly ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;

        public CreateTopicUseCaseShould()
        {
            _storage = new Mock<ICreateTopicStorage>();
            _forumExistsSetup = _storage.Setup(s => s.ForumExist(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            _createTopicSetup = _storage.Setup(s => s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            _getUserIdSetup = identity.Setup(s => s.UserId);

            _intentionManager = new Mock<IIntentionManager>();
            _intentionIsAllowedSetup = _intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));

            var validator = new Mock<IValidator<CreateTopicCommand>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _createTopic = new CreateTopicUseCase(validator.Object, _intentionManager.Object, _storage.Object, identityProvider.Object);
        }

        [Fact]
        public async Task ThrowIntentionmanagerException_WhentopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("30ff1d42-4e74-47c4-843e-b52eeef900a0");

            _intentionIsAllowedSetup.Returns(false);

            await _createTopic.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            _intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            var forumId = Guid.Parse("14ef39d4-44d4-4660-9c90-b836f53c40cb");

            _intentionIsAllowedSetup.Returns(true);
            _forumExistsSetup.ReturnsAsync(false);

            await _createTopic
                    .Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Forum Title"), CancellationToken.None))
                    .Should().ThrowAsync<ForumNotFoundException>();

            _storage.Verify(f => f.ForumExist(forumId, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMathcingForumExists()
        {
            Guid forumId = Guid.Parse("3ca4604c-c046-4ded-9b39-337d1a45dda2");
            Guid userId = Guid.Parse("9298f1de-e294-4fb6-a2f6-d3abd29a1d08");

            _intentionIsAllowedSetup.Returns(true);
            _forumExistsSetup.ReturnsAsync(true);
            _getUserIdSetup.Returns(userId);

            Topic expected = new Topic();
            _createTopicSetup.ReturnsAsync(expected);

            Topic actual = await _createTopic.Execute(new CreateTopicCommand(forumId, "Forum 1"), CancellationToken.None);
            actual.Should().Be(expected);

            _storage.Verify(s => s.CreateTopic(forumId, userId, "Forum 1", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}