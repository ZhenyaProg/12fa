using FluentAssertions;
using TFA.Application.UseCases.GetTopics;

namespace TFA.Application.Tests.GetTopics;

public class GetTopicsQueryValidatorShould
{
    private readonly GetTopicsQueryValidator _sut = new();
    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var query = new GetTopicsQuery(
                    Guid.Parse("7fffef56-b26c-4da9-ab54-42ef79c25f07"),
                    10,
                    5);
        _sut.Validate(query).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQueries()
    {
        var query = new GetTopicsQuery(
                    Guid.Parse("7fffef56-b26c-4da9-ab54-42ef79c25f07"),
                    10,
                    5);
        yield return new object[] { query with { ForumId = Guid.Empty } };
        yield return new object[] { query with { Skip = -23 } };
        yield return new object[] { query with { Take = -39 } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidQueries))]
    public void ReturnFailuure_WhenQueryIsInvalid(GetTopicsQuery query)
    {
        _sut.Validate(query).IsValid.Should().BeFalse();
    }
}