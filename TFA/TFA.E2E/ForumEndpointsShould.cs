using FluentAssertions;

namespace TFA.E2E
{
    public class ForumEndpointsShould : IClassFixture<ForumAPIApplicationFactory>
    {
        private readonly ForumAPIApplicationFactory _factory;

        public ForumEndpointsShould(ForumAPIApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnListOfForums()
        {
            using var httpClient = _factory.CreateClient();
            using var response = await httpClient.GetAsync("api/forums");

            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be("[]");
        }
    }
}