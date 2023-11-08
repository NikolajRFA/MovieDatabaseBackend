﻿using System.Security.Claims;
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
    public IActionResult GetBookmarks(int userId, int page = 0, int pageSize = 10)
    {
        var bookmarks = _dataService.GetBookmarks(userId);
        if (bookmarks.Count == 0)
        {
            return NotFound();
        }

        List<BookmarkDto> dtos = new();
        bookmarks.ForEach(bookmark =>
        {
            dtos.Add(new BookmarkDto
            {
                Url = GetUrl(nameof(GetBookmark), new { userId, bookmark.Id }),
                User = GetUrl(nameof(UsersController.GetUser), new { id = userId }),
                Title = GetUrl(nameof(TitlesController.GetTitle), new { tconst = bookmark.Tconst?.Trim() }),
                Person = GetUrl(nameof(PersonsController.GetPerson), new { nconst = bookmark.Nconst?.Trim() })
            });
        });

        return Ok(Paging(dtos, bookmarks.Count,
            new UserBookmarksPagingValues { UserId = userId, Page = page, PageSize = pageSize }, nameof(GetBookmarks)));
    }

    [HttpGet("{id}", Name = nameof(GetBookmark))]
    public IActionResult GetBookmark(int id, int userId)
    {
        var bookmark = _dataService.GetBookmark(id);
        if (bookmark == null || bookmark.UserId != userId) return NotFound();
        var dto = new BookmarkDto
        {
            Url = GetUrl(nameof(GetBookmark), new { userId, bookmark.Id }),
            User = GetUrl(nameof(UsersController.GetUser), new { id = userId }),
            Title = GetUrl(nameof(TitlesController.GetTitle), new { tconst = bookmark.Tconst?.Trim() }),
            Person = GetUrl(nameof(PersonsController.GetPerson), new { nconst = bookmark.Nconst?.Trim() })
        };
        return Ok(dto);
    }
    [Authorize]
    [HttpDelete("{id}", Name = nameof(DeleteBookmark))]
    public IActionResult DeleteBookmark(int id)
    {
        var userId = ExtractUserIdFromClaim();
        if (userId == null) return StatusCode(401);
        
        var bookmark = _dataService.GetBookmark(id);
        if (bookmark == null) return NotFound();
        if (bookmark.UserId != userId) return StatusCode(401);
        
        _dataService.DeleteBookmark(bookmark.Id);
        
        return Ok();
    }

    [HttpPost("title")]
    public IActionResult CreateTitleBookmark(BookmarkModel bookmarkModel)
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
    public IActionResult CreatePersonBookmark(BookmarkModel bookmarkModel)
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

    private class UserBookmarksPagingValues : PagingValues
    {
        public int UserId { get; set; }
    }
}