using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class CrewProfile : Profile
{
    public CrewProfile()
    {
        CreateMap<Crew, CrewDto>();
    }
}