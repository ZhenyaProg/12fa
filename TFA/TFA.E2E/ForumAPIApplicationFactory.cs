using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using TFA.Storage;

namespace TFA.E2E;

public class ForumAPIApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuraction = new ConfigurationBuilder()
             .AddInMemoryCollection(new Dictionary<string, string>()
             {
                 ["ConnectionStrings:Postgres"] = _dbContainer.GetConnectionString(),
             })
            .Build();
        builder.UseConfiguration(configuraction);
        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var forumDbComtext = new ForumDbContext(new DbContextOptionsBuilder<ForumDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await forumDbComtext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}