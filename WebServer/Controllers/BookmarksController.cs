using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers;

[Route("api/bookmarks")]
[ApiController]

public class BookmarksController : GenericControllerBase
{
    private readonly BookmarkDataService _dataService;

    public BookmarksController(BookmarkDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    [HttpGet("{Id}", Name = nameof(GetBookmarksWithUser))]
    public IActionResult GetBookmarksWithUser(int id, int page = 0, int pageSize = 10)
    {
        var bookmarks = _dataService.GetBookmarks(id);
        if (bookmarks.Count == 0)
        {
            return NotFound();
        }
        return Ok(Paging(bookmarks, bookmarks.Count, page, pageSize, nameof(GetBookmarksWithUser)));
    }
}