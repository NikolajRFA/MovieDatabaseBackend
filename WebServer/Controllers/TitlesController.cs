﻿using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DataTransferObjects;
using DataLayer.DbSets;
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
        return Ok(Paging(dtos, titles.count, new TitleSearchPagingValues{Q = q, Page = page, PageSize = pageSize },
            nameof(GetTitles)));
    }

    private TitleDto MapTitle(Title title)
    {
        var dto = Mapper.Map<TitleDto>(title);
        dto.Url = GetUrl(nameof(GetTitle), new { tconst = title.Tconst.Trim() });
        dto.Genres = Mapper.Map<List<GenreDto>>(title.Genre);
        return dto;
    }
}

internal class TitleSearchPagingValues : PagingValues
{
    public string?  Q { get; set; }
}