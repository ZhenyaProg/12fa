using FluentAssertions;
using TFA.Application.UseCases.CreateTopic;

namespace TFA.Application.Tests;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator _validator = new();

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var actual = _validator.Validate(new CreateTopicCommand(Guid.Parse("ec0f7bb5-95a0-40c0-8705-a26da097414b"), "Hello"));
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new CreateTopicCommand(Guid.Parse("67561173-8bbd-4e62-a00e-c8a6b1395ee3"), "Hello");
        yield return new object[] { validCommand with { ForumId = Guid.Empty }, nameof(CreateTopicCommand.ForumId), "Empty" };
        yield return new object[] { validCommand with { Title = string.Empty }, nameof(CreateTopicCommand.Title), "Empty" };
        yield return new object[] { validCommand with { Title = "   " }, nameof(CreateTopicCommand.Title), "Empty" };
        yield return new object[] {validCommand with { Title = string.Join("a", Enumerable.Range(0, 110)) }, nameof(CreateTopicCommand.Title), "Toolong" };
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand command, string expectedPropertyName, string expectedErrorCode)
    {
        var actual = _validator.Validate(command);
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should()
            .Contain(f => f.PropertyName == expectedPropertyName && f.ErrorCode == expectedErrorCode);
    }
}