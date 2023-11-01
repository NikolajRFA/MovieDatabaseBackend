using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>().ForMember(dest => dest.Genre, act => act.MapFrom(src => src.GenreName));
    }
}