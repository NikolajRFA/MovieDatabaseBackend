using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;
using WebServer.Models;

namespace WebServer.Controllers;

[Route("api/user/{userId:int}/ratings")]
[ApiController]
public class RatingController : GenericControllerBase
{
    private readonly RatingDataService _dataService;
    private readonly IMapper _mapper;

    public RatingController(RatingDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator)
    {
        _dataService = dataService;
        _mapper = mapper;
    }


    [HttpGet(Name = nameof(GetRatings))]

    public IActionResult GetRatings(int userId, int page = 0, int pageSize = 10)
    {
        var ratings = _dataService.GetRatings(userId);
        if (ratings.Count == 0)
        {
            return NotFound();
        }

        List <RatingDto> ratingDtos = new();
        ratings.ForEach(rating =>
        {
            ratingDtos.Add(new RatingDto
            {
                Url = GetUrl(nameof(GetRatings), new {userId, rating.Id}),
                Tconst = rating.Tconst,
                Id = rating.Id,
                Rating = rating.ThisRating,
                Date = rating.Date

            });
        });

        return Ok(Paging(ratingDtos, ratings.Count, page, pageSize, nameof(GetRatings)));
    }

    [HttpGet("{tconst}", Name = nameof(GetRating))]
    public IActionResult GetRating(string tconst, int userId)
    {
        var rating = _dataService.GetRating(tconst);
        if (rating == null) { return NotFound(); }
        var dto = new RatingDto
        {
            Url = GetUrl(nameof(GetRating), new { userId, rating.Id }),
            Id = rating.Id,
            Tconst = rating.Tconst,
            Rating = rating.ThisRating,
            Date = rating.Date
        };
        return Ok(dto);
    }


    [HttpPost("{rating})")]

    public IActionResult CreateRating(RatingModel ratingModel)
    {
        try
        {
            _dataService.CreateRating(ratingModel.id, ratingModel.tconst, ratingModel.rating);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(403);
        }
    }

   
    [HttpPut]
    public IActionResult UpdateRating(RatingModel ratingModel)
    {
        try
        {
            _dataService.UpdateRating(ratingModel.id, ratingModel.tconst, ratingModel.rating);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(403);
        }
    }
   
   
}
   
