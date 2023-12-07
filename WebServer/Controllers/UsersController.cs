using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebServer.DataTransferObjects;
using WebServer.Models;
using WebServiceToken.Services;

namespace WebServer.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : GenericControllerBase
{
    private readonly UserDataService _dataService;
    private readonly Hashing _hashing;
    private readonly IConfiguration _configuration;

    public UsersController(UserDataService dataService, LinkGenerator linkGenerator, IMapper mapper,
        Hashing hashing, IConfiguration configuration) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
        _hashing = hashing;
        _configuration = configuration;
    }

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

    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        if (_dataService.GetUser(id) == null) return NotFound();
        if (User.IsInRole("Admin"))
        {
            if (_dataService.DeleteUser(id)) return Ok("Deleted");
        }
        else
        {
            if (id != UserId) return Unauthorized();
            if (_dataService.DeleteUser(id)) return Ok("Deleted");
        }

        return StatusCode(500);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public IActionResult UpdateUser(int id, UpdateUserModel model)
    {
        var user = _dataService.GetUser(id);
        if (user == null) return NotFound();
        if (user.Id != UserId) return Unauthorized();
        var (hashedPwd, salt) = _hashing.Hash(model.Password);
        var updatedUser = _dataService.UpdateUser(id, model.Username, model.Email, hashedPwd, salt, model.Role);
        var dto = MapUser(updatedUser);
        return Ok(dto);
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserModel model)
    {
        if (_dataService.GetUser(model.Username) != null)
        {
            return BadRequest();
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            return BadRequest();
        }

        var (hashedPwd, salt) = _hashing.Hash(model.Password);

        var user = _dataService.CreateUser(model.Username, model.Email, hashedPwd, salt, model.Role);

        if (user == null) return BadRequest();

        var dto = MapUser(user);
        return Created(dto.Url, dto);
    }

    // Naming - Henrik notes that the naming of the POST below is violating the idea of naming as it's not a resource
    [HttpPost("login")]
    public IActionResult Login(UserLoginModel model)
    {
        var user = _dataService.GetUser(model.Username);
        if (user == null)
        {
            return BadRequest();
        }

        if (!_hashing.Verify(model.Password, user.Password, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var secret = _configuration.GetSection("Auth:Secret").Value;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { user.Username, user.Id, token = jwt });
    }

    private UserDto MapUser(User user)
    {
        var dto = Mapper.Map<UserDto>(user);
        dto.Url = GetUrl(nameof(GetUser), new { user.Id });
        dto.Bookmarks = GetUrl(nameof(UserBookmarksController.GetBookmarks), new { userId = user.Id });
        dto.Searches = GetUrl(nameof(UserSearchController.GetSearches), new { userId = user.Id });
        dto.Ratings = GetUrl(nameof(RatingsController.GetRatings), new { userId = user.Id });
        return dto;
    }
}