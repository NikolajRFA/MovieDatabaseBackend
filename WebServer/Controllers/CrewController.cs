using System.Resources;
using AutoMapper;
using AutoMapper.Execution;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

[Route("api/crew")]
[ApiController]
public class CrewController : GenericControllerBase
{
    private readonly CrewDataService _dataService;

    public CrewController(CrewDataService titleDataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = titleDataService;
    }

    [HttpGet("{tconst}", Name = nameof(GetCrew))]
    public IActionResult GetCrew(string tconst, int page = 0, int pageSize = 10)
    {
        var (crew, total) = _dataService.GetCrew(tconst, page, pageSize);
        List<CrewDto> dtos = new();
        foreach (var member in crew)
        {
            var dto = MapCrew(member);
            dtos.Add(dto);
        }

        return Ok(Paging(dtos, total, new CrewPagingValues { Tconst = tconst, Page = page, PageSize = pageSize },
            nameof(GetCrew)));
    }

    [HttpGet("id/{id}", Name = nameof(GetCrewSingle))]
    public IActionResult GetCrewSingle(int id)
    {
        var crew = _dataService.GetCrewSingle(id);
        if (crew == null) return NotFound();
        var dto = MapCrew(crew);
        return Ok(dto);
    }

    private CrewDto MapCrew(Crew crew)
    {
        var dto = Mapper.Map<CrewDto>(crew);
        dto.Url = GetUrl(nameof(GetCrewSingle), new { crew.Id });
        dto.Person = GetUrl(nameof(PersonsController.GetPerson), new { nconst = crew.Nconst.Trim() });
        dto.Title = GetUrl(nameof(TitlesController.GetTitle), new { tconst = crew.Tconst.Trim() });
        return dto;
    }

    private class CrewPagingValues : PagingValues
    {
        public string Tconst { get; set; }
    }
}