using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : GenericControllerBase
{
    private readonly UserDataService _dataService;
    private readonly IMapper _mapper;

    public UserController(UserDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator)
    {
        _dataService = dataService;
        _mapper = mapper;
    }

    // TODO: Add url references to searches and ratings.
    [HttpGet("{id:int}", Name = nameof(GetUser))]
    public IActionResult GetUser(int id)
    {
        var user = _dataService.GetUser(id);
        if (user == null) return NotFound();

        var dto = _mapper.Map<UserDto>(user);
        dto.Url = GetUrl(nameof(GetUser), new { user.Id });
        dto.Bookmarks = GetUrl(nameof(UserBookmarksController.GetBookmarks), new { userId = user.Id });
        dto.Searches = "TODO";
        dto.Ratings = GetUrl(nameof(RatingController.GetRatingsFromUser), new { userId = user.Id });
        return Ok(dto);
    }

    [HttpGet(Name = nameof(GetUsers))]
    public IActionResult GetUsers(int page = 0, int pageSize = 10)
    {
        var (users, count) = _dataService.GetUsers(page, pageSize);

        List<UserDto> dtos = new();
        foreach (var user in users)
        {
            var dto = _mapper.Map<UserDto>(user);
            dto.Url = GetUrl(nameof(GetUser), new { user.Id });
            dto.Bookmarks = GetUrl(nameof(UserBookmarksController.GetBookmarks), new { userId = user.Id });
            dto.Searches = "TODO";
            dto.Ratings = GetUrl(nameof(RatingController.GetRatingsFromUser), new { userId = user.Id });
            dtos.Add(dto);
        }

        return Ok(Paging(dtos, count, page, pageSize, nameof(GetUsers)));
    }
}