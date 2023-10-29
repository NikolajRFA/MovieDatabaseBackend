using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}