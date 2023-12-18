using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;
using WebServer.Models;
using User = DataLayer.DbSets.User;

namespace WebServer.Controllers;

[Route("api/users/{userId:int}/ratings")]
[ApiController]
public class RatingsController : GenericControllerBase
{
    private readonly RatingDataService _dataService;

    public RatingsController(RatingDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    [Authorize]
    [HttpDelete]
    public IActionResult DeleteRating(int userId, string tconst)
    {
        if (UserId != userId) return Unauthorized();
        var rating = _dataService.GetRating(tconst, userId);
        if (rating == null) return NotFound();
        _dataService.DeleteRating(rating.Id, rating.Tconst);
        return Ok();
    }

    [HttpGet(Name = nameof(GetRatings))]
    public IActionResult GetRatings(int userId, int page = 0, int pageSize = 3)
    {
        var (ratings, count) = _dataService.GetRatings(userId, page, pageSize);

        List<RatingDto> ratingDtos = new();
        foreach (var rating in ratings)
        {
            var dto = MapRating(rating, userId);
            ratingDtos.Add(dto);
            GetUrl(nameof(GetRatings), new { userId });
        }

        return Ok(Paging(ratingDtos, count,
            new RatingsPagingValues { Page = page, PageSize = pageSize, UserId = userId }, nameof(GetRatings)));
    }

    [HttpGet("{tconst}", Name = nameof(GetRating))]
    public IActionResult GetRating(string tconst, int userId)
    {
        var rating = _dataService.GetRating(tconst, userId);
        if (rating == null)
        {
            return NotFound();
        }

        var dto = MapRating(rating, userId);
        return Ok(dto);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateRating(RatingModel ratingModel)
    {
        try
        {
            _dataService.CreateRating(UserId!.Value, ratingModel.Tconst, ratingModel.Rating);
            return Ok();
        }
        catch (Exception)
        {
            return Forbid();
        }
    }

    [Authorize]
    [HttpPut]
    public IActionResult UpdateRating(RatingModel ratingModel, [FromRoute] int userId)
    {
        if (userId != UserId) return Unauthorized();
        var rating = _dataService.GetRating(ratingModel.Tconst, userId);
        if (rating == null) return NotFound();

        _dataService.UpdateRating(UserId!.Value, ratingModel.Tconst, ratingModel.Rating);

        rating = _dataService.GetRating(ratingModel.Tconst, userId);
        
        var dto = MapRating(rating!, UserId.Value);
        GetUrl(nameof(GetRating), new { UserId });

        return Ok(dto);
    }

    private RatingDto MapRating(Rating rating, int userId)
    {
        var ratingDto = Mapper.Map<RatingDto>(rating);
        ratingDto.Url = GetUrl(nameof(GetRating), new { userId, Tconst = rating.Tconst.Trim() });
        ratingDto.User = GetUrl(nameof(UsersController.GetUser), new { rating.Id });
        ratingDto.Tconst = GetUrl(nameof(TitlesController.GetTitle), new { Tconst = rating.Tconst.Trim() });
        ratingDto.Rating = rating.ThisRating;
        ratingDto.Date = rating.Date;
        return ratingDto;
    }

    private class RatingsPagingValues : PagingValues
    {
        public int UserId { get; set; }
    }
}