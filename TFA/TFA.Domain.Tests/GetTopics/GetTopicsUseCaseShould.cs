using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Application.Exceptions;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;
using TFA.Application.UseCases.GetTopics;

namespace TFA.Application.Tests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly Mock<IGetTopicsStorage> _storage;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicsSetup;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumsSetup;
    private readonly GetTopicsUseCase _sut;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _storage = new Mock<IGetTopicsStorage>();
        _getTopicsSetup = _storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        var getForumStorage = new Mock<IGetForumsStorage>();
        _getForumsSetup = getForumStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(validator.Object, _storage.Object, getForumStorage.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoForum()
    {
        var forumId = Guid.Parse("b899bac8-05b0-4003-962a-489f82a52d79");

        _getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("4c05ea9d-2d21-4b75-acc5-717421ad1172")}});

        var query = new GetTopicsQuery(forumId, 0, 1);
        await _sut.Invoking(s => s.Execute(query, CancellationToken.None))
                .Should()
                .ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
    {
        var forumId = Guid.Parse("03f63e78-291c-429b-8d7e-980825073563");

        _getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("03f63e78-291c-429b-8d7e-980825073563") } });
        var expectedResources = new Topic[] { new() };
        var expectedTotalCount = 6;
        _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) = await _sut.Execute(
            new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once());
    }
}