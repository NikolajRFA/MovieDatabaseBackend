using AutoMapper;
using DataLayer.DataTransferObjects;
using DataLayer.DbSets;

namespace WebServer.DataTransferObjects.Profiles;

public class BookmarkProfile : Profile
{
    public BookmarkProfile()
    {
        CreateMap<Bookmark, BookmarkDto>();
    }
}