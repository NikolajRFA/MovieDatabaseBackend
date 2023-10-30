using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

[Route("api/titles")]
[ApiController]

public class TitlesController : GenericControllerBase
{
    private readonly TitleDataService _dataService;
    private readonly IMapper _mapper;

    public TitlesController(TitleDataService titleDataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator)
    {
        _dataService = titleDataService;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetTitles))]
    public IActionResult GetTitles(int page = 0, int pageSize = 10)
    {
        var titles = _dataService.GetTitles(page, pageSize);
        List<TitleDto> dtos = new();
        titles.titles.ForEach(title =>
        {
            dtos.Add(new TitleDto
            {
                Url = GetUrl(nameof(GetTitle), new{ tconst = title.Tconst.Trim()}),
                Title = title.OriginalTitle,
                TitleType = title.TitleType,
                Poster = title.Poster,
                StartYear = title.StartYear,
                EndYear = title.EndYear,
                IsAdult = title.IsAdult,
                RunTimeMinutes = title.RuntimeMinutes,
                AverageRating = title.AverageRating,
                NumVotes = title.NumVotes,
                Plot = title.Plot
                // Missing PersonalRatingDto, GenreDto, PersonDto
            });
        });
        return Ok(Paging(dtos, titles.count, new PagingValues{Page = page, PageSize = pageSize}, nameof(GetTitles)));
    }

    [HttpGet("{tconst}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string tconst)
    {
        var title = _dataService.GetTitle(tconst);
        var titleDto = new TitleDto
        {
            Url = GetUrl(nameof(GetTitle), new{ tconst = title.Tconst.Trim()}),
            Title = title.OriginalTitle,
            TitleType = title.TitleType,
            Poster = title.Poster,
            StartYear = title.StartYear,
            EndYear = title.EndYear,
            IsAdult = title.IsAdult,
            RunTimeMinutes = title.RuntimeMinutes,
            AverageRating = title.AverageRating,
            NumVotes = title.NumVotes,
            Plot = title.Plot
        };
        return Ok(titleDto);
    }
}