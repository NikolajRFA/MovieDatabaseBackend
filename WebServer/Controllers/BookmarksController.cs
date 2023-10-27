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

    [HttpDelete("{Id}")]
    public IActionResult DeleteBookmarkWithUser(int id)
    {
        _dataService.DeleteBookmark(id);
        return Ok();
    }
}