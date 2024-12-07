using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;
using TFA.Application.UseCases.GetTopics;
using TFA.Storage.UseCases;

namespace TFA.Storage.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                           .AddScoped<IGetForumsStorage, GetForumsStorage>()
                           .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                           .AddScoped<IMomentProvider, MomentProvider>()
                           .AddScoped<IGuidFactory, GuidFactory>()
                           .AddDbContextPool<ForumDbContext>(options =>
                               options.UseNpgsql(connectionString));
        }
    } 
}