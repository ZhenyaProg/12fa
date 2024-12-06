using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;
using TFA.Storage.UseCases;

namespace TFA.Storage.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<IGuidFactory, GuidFactory>()
                           .AddScoped<IMomentProvider, MomentProvider>()
                           .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                           .AddScoped<IGetForumsStorage, GetForumsStorage>()
                           .AddDbContextPool<ForumDbContext>(options =>
                               options.UseNpgsql(connectionString));
        }
    } 
}