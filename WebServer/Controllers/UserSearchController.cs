using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Controllers;
using WebServer.DataTransferObjects;


namespace WebServer.Controllers;

[Route("api/users/{userId:int}/searches")]
[ApiController]
[Authorize]
public class UserSearchController : GenericControllerBase
{
    private readonly UserSearchDataService _dataService;

    public UserSearchController(UserSearchDataService dataService, LinkGenerator linkGenerator, IMapper mapper) :
        base(
            linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    [HttpDelete]
    public IActionResult DeleteSearch(int userId, string searchPhrase)
    {
        if (userId != UserId) return Unauthorized();
        _dataService.DeleteSearch(userId, searchPhrase);
        return Ok("Search Deleted");
    }

    [HttpDelete("clear")]
    public IActionResult DeleteSearches(int userId)
    {
        if (userId != UserId) return Unauthorized();
        _dataService.DeleteSearches(userId);
        return Ok("Searches Deleted");
    }

    [HttpGet(Name = nameof(GetSearches))]
    public IActionResult GetSearches(int userId, int page = 0, int pageSize = 10)
    {
        if (userId != UserId) return Unauthorized();
        var (searches, count) = _dataService.GetSearches(userId, page, pageSize);
        List<SearchDto> searchDtos = new List<SearchDto>();
        foreach (var search in searches)
        {
            var dto = MapSearch(search);
            searchDtos.Add(dto);
        }

        return Ok(Paging(searchDtos, count, new
            UserSearchPagingValues { UserId = userId, Page = page, PageSize = pageSize }, nameof(GetSearches)));
    }


    [HttpGet("single", Name = nameof(GetSearch))]
    public ActionResult GetSearch(int userId, string searchPhrase)
    {
        if (userId != UserId) return Unauthorized();
        var search = _dataService.GetSearch(userId, searchPhrase);
        if (search == null) return NotFound();
        var dto = MapSearch(search);
        return Ok(dto);
    }

    private SearchDto MapSearch(Search search)
    {
        var searchDto = Mapper.Map<SearchDto>(search);
        searchDto.Url = GetUrl(nameof(GetSearch), new { userId = search.Id, search.SearchPhrase });
        searchDto.User = GetUrl(nameof(UsersController.GetUser), new { search.Id });
        searchDto.SearchPhrase = search.SearchPhrase;
        searchDto.Date = search.Date;
        return searchDto;
    }

    private class UserSearchPagingValues : PagingValues
    {
        public int UserId { get; set; }
    }
}