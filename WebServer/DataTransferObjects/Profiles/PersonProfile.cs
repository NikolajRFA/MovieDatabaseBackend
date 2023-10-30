using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonDto>();
    }
}