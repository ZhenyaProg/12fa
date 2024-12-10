using AutoMapper;
using TFA.API.Contracts;
using TFA.Application.Models;

namespace TFA.API.Mapping
{
    public class APIProfile : Profile
    {
        public APIProfile()
        {
            CreateMap<Forum, ForumResponse>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.Id))
                .ForMember(d => d.Title, s => s.MapFrom(f => f.Title));

            CreateMap<Topic, TopicResponse>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.Id))
                .ForMember(d => d.Title, s => s.MapFrom(t => t.Title))
                .ForMember(d => d.CreatedDate, s => s.MapFrom(t => t.CreatedDate));
        }
    }
}