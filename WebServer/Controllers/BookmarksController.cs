using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

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

    [HttpPost("title")]
    public IActionResult CreateTitleBookmarkWithUser(BookmarkModel bookmarkModel)
    {
        try
        {
            _dataService.CreateMovieBookmark(bookmarkModel.UserId, bookmarkModel.TitlePersonId);
            return Ok();
        }
        catch (Exception e)
        {
            // Should be Forbid() but authentication isn't specified so it won't work
            return StatusCode(403);
        }
    }
    [HttpPost("person")]
    public IActionResult CreatePersonBookmarkWithUser(BookmarkModel bookmarkModel)
    {
        try
        {
            _dataService.CreatePersonBookmark(bookmarkModel.UserId, bookmarkModel.TitlePersonId);
            return Ok();
        }
        catch (Exception e)
        {
            // Should be Forbid() but authentication isn't specified so it won't work
            return StatusCode(403);
        }
    }

    [HttpDelete("{Id}")]
    public IActionResult DeleteBookmarkWithUser(int id)
    {
        _dataService.DeleteBookmark(id);
        return Ok();
    }
}