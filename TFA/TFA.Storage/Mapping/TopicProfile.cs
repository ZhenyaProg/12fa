using AutoMapper;
using TFA.Application.Models;

namespace TFA.Storage.Mapping;

internal class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<TopicEntity, Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(t => t.Id))
            .ForMember(d => d.ForumId, s => s.MapFrom(t => t.ForumId))
            .ForMember(d => d.AuthorId, s => s.MapFrom(t => t.AuthorId))
            .ForMember(d => d.Title, s => s.MapFrom(t => t.Title))
            .ForMember(d => d.CreatedDate, s => s.MapFrom(t => t.CreatedDate));
    }
}