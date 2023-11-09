using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class EpisodeProfile : Profile
{
    public EpisodeProfile()
    {
        CreateMap<IsEpisodeOf, EpisodeDto>()
            .ForMember(dest => dest.Season, act => act.MapFrom(src => src.SeasonNumber))
            .ForMember(dest => dest.Episode, act => act.MapFrom(src => src.EpisodeNumber));
    }
    
}