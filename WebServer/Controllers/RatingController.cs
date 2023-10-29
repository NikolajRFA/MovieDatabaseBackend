using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;
using WebServer.Models;

namespace WebServer.Controllers;

[Route("api/user/{userId:int}/rating")]
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

    [HttpGet]

    public IActionResult GetRatingsFromUser(int userId)
    {
        var ratings = _dataService.GetRating(userId);
        if (ratings.Count == 0)
        {
            return NotFound();
        }

        List <RatingDto> ratingDtos = new();
        ratings.ForEach(rating =>
        {
            ratingDtos.Add(new RatingDto
            {
                Tconst = rating.Tconst,
                Id = rating.Id,
                Rating = rating.ThisRating,
                Date = rating.Date

            });
        });

        return Ok(ratingDtos);
    }
}

