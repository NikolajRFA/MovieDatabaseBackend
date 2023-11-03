using AutoMapper;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles
{
    public class UserSearchProfile : Profile
    {
        public UserSearchProfile()
        {
            CreateMap<Search, SearchDto>();
        }
    }
}
