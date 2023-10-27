using AutoMapper;
using DataLayer.DataTransferObjects;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        CreateMap<Rating, RatingDto>();
    }
}