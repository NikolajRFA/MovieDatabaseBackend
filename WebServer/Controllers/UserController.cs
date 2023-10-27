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

    public UserController(UserDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    // TODO: Add url references to bookmarks, searches and ratings.
    [HttpGet("{id:int}", Name = nameof(GetUser))]
    public IActionResult GetUser(int id)
    {
        var user = _dataService.GetUser(id);
        if (user == null) return NotFound();
        var dto = new UserDto
        {
            Url = GetUrl(nameof(GetUser), new { user.Id }),
            Username = user.Username,
            Email = user.Email,
            Searches = "TODO",
            Bookmarks = GetUrl(nameof(UserBookmarksController.GetBookmarks), new { user.Id }),
            Ratings = "TODO"
        };
        return Ok(dto);
    }
}