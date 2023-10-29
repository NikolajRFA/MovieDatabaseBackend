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
    private readonly IMapper _mapper;

    public BookmarksController(BookmarkDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator)
    {
        _dataService = dataService;
        _mapper = mapper;
    }

    [HttpDelete("{Id}")]
    public IActionResult DeleteBookmarkWithUser(int id)
    {
        _dataService.DeleteBookmark(id);
        return Ok();
    }
}