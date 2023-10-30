using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;
using WebServer.Models;

namespace WebServer.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : GenericControllerBase
{
    private readonly UserDataService _dataService;

    public UsersController(UserDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    // TODO: Add url references to searches and ratings.
    [HttpGet("{id:int}", Name = nameof(GetUser))]
    public IActionResult GetUser(int id)
    {
        var user = _dataService.GetUser(id);
        if (user == null) return NotFound();

        var dto = MapUser(user);
        return Ok(dto);
    }

    [HttpGet(Name = nameof(GetUsers))]
    public IActionResult GetUsers(int page = 0, int pageSize = 10)
    {
        var (users, count) = _dataService.GetUsers(page, pageSize);

        List<UserDto> dtos = new();
        foreach (var user in users)
        {
            var dto = MapUser(user);
            dtos.Add(dto);
        }

        return Ok(Paging(dtos, count, new PagingValues { Page = page, PageSize = pageSize }, nameof(GetUsers)));
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserModel createUser)
    {
        var user = _dataService.CreateUser(createUser.Username, createUser.Email, createUser.Password);
        if (user == null) return BadRequest();

        var dto = MapUser(user);
        return Ok(dto);
    }

    [HttpDelete("{id:int}")] // Something (authentication) should be added here so a user only can delete their own account.
    public IActionResult DeleteUser(int id)
    {
        if (_dataService.GetUser(id) == null) return NotFound();
        if (_dataService.DeleteUser(id))
        {
            return Ok();
        }

        return StatusCode(500);
    }

    private UserDto MapUser(User user)
    {
        var dto = Mapper.Map<UserDto>(user);
        dto.Url = GetUrl(nameof(GetUser), new { user.Id });
        dto.Bookmarks = GetUrl(nameof(UserBookmarksController.GetBookmarks), new { userId = user.Id });
        dto.Searches = "TODO";
        dto.Ratings = GetUrl(nameof(RatingsController.GetRatings), new { userId = user.Id });
        return dto;
    }
}