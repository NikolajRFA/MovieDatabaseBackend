using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class AliasProfile : Profile
{
    public AliasProfile()
    {
        CreateMap<Alias, AliasDto>();
    }
}