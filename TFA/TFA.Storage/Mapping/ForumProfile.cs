using AutoMapper;
using TFA.Application.Models;

namespace TFA.Storage.Mapping;

internal class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<ForumEntity, Forum>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.Id))
            .ForMember(d => d.Title, s => s.MapFrom(f => f.Title));
    }
}