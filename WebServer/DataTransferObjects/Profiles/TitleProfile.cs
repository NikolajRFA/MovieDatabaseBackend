using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class TitleProfile : Profile
{
    public TitleProfile()
    {
        CreateMap<Title, TitleDto>()
            .ForMember(dest => dest.Title, act => act.MapFrom(src => src.PrimaryTitle));
        CreateMap<Title, MovieSearchDropdownDto>()
            .ForMember(dest => dest.Title, act => act.MapFrom(src => src.PrimaryTitle));
    }
}