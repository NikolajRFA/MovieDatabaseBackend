using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class ProfessionProfile : Profile
{
    public ProfessionProfile()
    {
        CreateMap<Profession, ProfessionDto>().ForMember(dest => dest.Profession, act => act.MapFrom(src => src.ProfessionName));
    }
}