using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class ProfessionProfile : Profile
{
    public ProfessionProfile()
    {
        CreateMap<Profession, ProfessionDto>();
    }
}