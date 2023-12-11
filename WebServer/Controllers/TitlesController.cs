using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DataTransferObjects;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

[Route("api/titles")]
[ApiController]
public class TitlesController : GenericControllerBase
{
    private readonly TitleDataService _dataService;

    public TitlesController(TitleDataService titleDataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = titleDataService;
    }
    
    [HttpGet("{tconst}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string tconst)
    {
        var title = _dataService.GetTitle(tconst);
        var titleDto = MapTitle(title);
        return Ok(titleDto);
    }

    [HttpGet("{tconst}/episodes", Name = nameof(GetEpisodesOfSeries))]
    public IActionResult GetEpisodesOfSeries(string tconst, int page = 0, int pageSize = 10, int season = 0)
    {
        var (episodes, total) = _dataService.GetEpisodes(tconst, page, pageSize, season);
        if (episodes == null) return NotFound();
        List<EpisodeDto> episodeDtos = new();
        foreach (var episode in episodes)
        {
            var dto = Mapper.Map<EpisodeDto>(episode);
            dto.Title = episode.Title.PrimaryTitle;
            dto.ParentUrl = GetUrl(nameof(GetTitle), new { tconst = episode.ParentTconst.Trim() });
            dto.EpisodeUrl = GetUrl(nameof(GetTitle), new { tconst = episode.Tconst.Trim() });
            episodeDtos.Add(dto);
        }

        return Ok(Paging(episodeDtos, total,
            new TconstPagingValues { Tconst = tconst, Page = page, PageSize = pageSize }, nameof(GetEpisodesOfSeries)));
    }
    

    [HttpGet(Name = nameof(GetTitles))]
    public IActionResult GetTitles(int id = 1, string? q = null, int page = 0, int pageSize = 10)
    {
        var titles = q == null
            ? _dataService.GetTitles(page, pageSize)
            : _dataService.GetTitlesSearch(id, q, page, pageSize);

        List<TitleDto> dtos = new();
        titles.titles.ForEach(title =>
        {
            var dto = MapTitle(title);
            dtos.Add(dto);
        });
        return Ok(Paging(dtos, titles.count, new TitleSearchPagingValues { Q = q, Page = page, PageSize = pageSize },
            nameof(GetTitles)));
    }

    [HttpGet("dropdown")]
    public IActionResult GetTitlesForDropdown(string q, int dropdownSize = 5)
    {
        var titles = _dataService.GetTitlesSearchForDropdown(q, dropdownSize);
        List<MovieSearchDropdownDto> dtos = new();
        titles.titles.ForEach(title =>
        {
            var dto = Mapper.Map<MovieSearchDropdownDto>(title);
            dto.Url = GetUrl(nameof(GetTitle), new { tconst = title.Tconst.Trim() });
            dto.PersonDtos = new();
            title.Crew
                .OrderBy(x => x.Ordering)
                .Take(2)
                .Select(x => x.Person)
                .ToList()
                .ForEach(person =>
                {
                    var personDto = Mapper.Map<PersonNameDto>(person);
                    personDto.Url = GetUrl(nameof(PersonsController.GetPerson), new { nconst = person.Nconst.Trim() });
                    dto.PersonDtos.Add(personDto);
                });
            dtos.Add(dto);
        });
        return Ok(dtos);
    }
    [HttpGet("results", Name = nameof(GetTitlesForSearch))]
    public IActionResult GetTitlesForSearch(string q, int page = 0, int pageSize = 10)
    {
        var titles = _dataService.GetTitlesSearch(UserId, q, page, pageSize);
        List<MovieSearchResultDto> dtos = new();
        titles.titles.ForEach(title =>
        {
            var dto = Mapper.Map<MovieSearchResultDto>(title);
            dto.Url = GetUrl(nameof(GetTitle), new { tconst = title.Tconst.Trim() });
            dto.PersonDtos = new();
            title.Crew
                .OrderBy(x => x.Ordering)
                .Take(2)
                .Select(x => x.Person)
                .ToList()
                .ForEach(person =>
                {
                    var personDto = Mapper.Map<PersonNameDto>(person);
                    personDto.Url = GetUrl(nameof(PersonsController.GetPerson), new { nconst = person.Nconst.Trim() });
                    dto.PersonDtos.Add(personDto);
                });
            dtos.Add(dto);
        });
        return Ok(Paging(dtos, titles.count, new TitleSearchPagingValues { Q = q, Page = page, PageSize = pageSize },
            nameof(GetTitlesForSearch)));
    }

    [HttpGet("{tconst}/aliases", Name = nameof(GetTitleAliases))]
    public IActionResult GetTitleAliases(string tconst, int page = 0, int pageSize = 10)
    {
        var (aliases, total) = _dataService.GetTitleAliases(tconst, page, pageSize);
        List<AliasDto> dtos = new();
        foreach (var alias in aliases)
        {
            var dto = Mapper.Map<AliasDto>(alias);
            dto.TitleUrl = GetUrl(nameof(GetTitle), new { tconst = alias.Tconst.Trim() });
            dtos.Add(dto);
        }

        return Ok(Paging(dtos, total, new TconstPagingValues { Tconst = tconst, Page = page, PageSize = pageSize },
            nameof(GetTitleAliases)));
    }

    private class TitleSearchPagingValues : PagingValues
    {
        public string? Q { get; set; }
    }

    private class TconstPagingValues : PagingValues
    {
        public string Tconst { get; set; }
    }
}