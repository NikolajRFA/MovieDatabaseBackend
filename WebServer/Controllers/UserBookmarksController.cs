using System.Security.Claims;
using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;
using WebServer.Models;

namespace WebServer.Controllers;

[Route("api/users/{userId:int}/bookmarks")]
[ApiController]
[Authorize]
public class UserBookmarksController : GenericControllerBase
{
    private readonly BookmarkDataService _dataService;

    public UserBookmarksController(BookmarkDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    [HttpGet(Name = nameof(GetBookmarks))]
    public IActionResult GetBookmarks(int page = 0, int pageSize = 10)
    {
        var bookmarks = _dataService.GetBookmarks(UserId!.Value);
        if (bookmarks.Count == 0)
        {
            return NotFound();
        }

        List<BookmarkDto> dtos = new();
        bookmarks.ForEach(bookmark =>
        {
            dtos.Add(new BookmarkDto
            {
                Url = GetUrl(nameof(GetBookmark), new { UserId, bookmark.Id }),
                User = GetUrl(nameof(UsersController.GetUser), new { id = UserId }),
                Tconst = GetUrl(nameof(TitlesController.GetTitle), new { tconst = bookmark.Tconst?.Trim() }),
                Nconst = GetUrl(nameof(PersonsController.GetPerson), new { nconst = bookmark.Nconst?.Trim() }),
                PersonName = bookmark.Person?.Name,
                Title = bookmark.Title?.PrimaryTitle
            });
        });

        return Ok(Paging(dtos, bookmarks.Count,
            new UserBookmarksPagingValues { UserId = UserId.Value, Page = page, PageSize = pageSize }, nameof(GetBookmarks)));
    }

    [HttpGet("{id}", Name = nameof(GetBookmark))]
    public IActionResult GetBookmark(int id)
    {
        var bookmark = _dataService.GetBookmark(id);
        if (bookmark == null) return NotFound();
        if (bookmark.UserId != UserId) return Unauthorized();
        var dto = new BookmarkDto
        {
            Url = GetUrl(nameof(GetBookmark), new { UserId, bookmark.Id }),
            User = GetUrl(nameof(UsersController.GetUser), new { id = UserId }),
            Title = GetUrl(nameof(TitlesController.GetTitle), new { tconst = bookmark.Tconst?.Trim() }),
            Nconst = GetUrl(nameof(PersonsController.GetPerson), new { nconst = bookmark.Nconst?.Trim() })
        };
        return Ok(dto);
    }
    
    [HttpDelete("{id:int}", Name = nameof(DeleteBookmark))]
    public IActionResult DeleteBookmark(int id)
    {
        var bookmark = _dataService.GetBookmark(id);
        if (bookmark == null) return NotFound();
        if (bookmark.UserId != UserId) return Unauthorized();
        
        _dataService.DeleteBookmark(bookmark.Id);
        
        return Ok();
    }

    [HttpPost("title")]
    public IActionResult CreateTitleBookmark(BookmarkModel bookmarkModel)
    {
        try
        {
            _dataService.CreateMovieBookmark(UserId!.Value, bookmarkModel.TitlePersonId);
            return Ok();
        }
        catch (Exception e)
        {
            return Forbid();
        }
    }

    [HttpPost("person")]
    public IActionResult CreatePersonBookmark(BookmarkModel bookmarkModel)
    {
        try
        {
            _dataService.CreatePersonBookmark(UserId!.Value, bookmarkModel.TitlePersonId);
            return Ok();
        }
        catch (Exception e)
        {
            return Forbid();
        }
    }

    private class UserBookmarksPagingValues : PagingValues
    {
        public int UserId { get; set; }
    }
}