using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Mvc;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

[Route("api/persons")]
[ApiController]
public class PersonsController : GenericControllerBase
{
    private readonly PersonDataService _dataService;

    public PersonsController(PersonDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }

    [HttpGet("{nconst}", Name = nameof(GetPerson))]
    public IActionResult GetPerson(string nconst)
    {
        var person = _dataService.GetPerson(nconst);
        if (person == null) return NotFound();

        return Ok(MapPerson(person));
    }

    [HttpGet(Name = nameof(GetPersons))]
    public IActionResult GetPersons(int page = 0, int pageSize = 10)
    {
        var (persons, total) = _dataService.GetPersons(page, pageSize);
        List<PersonDto> dtos = new();
        foreach (var person in persons)
        {
            dtos.Add(MapPerson(person));
        }

        return Ok(Paging(dtos, total, new PagingValues { Page = page, PageSize = pageSize }, nameof(GetPersons)));
    }

    [HttpGet("{nconst}/titles", Name = nameof(GetTitlesFromPerson))]
    public IActionResult GetTitlesFromPerson(string nconst, int page = 0, int pageSize = 10)
    {
        var (titles, total) = _dataService.GetTitlesFromPerson(nconst, page, pageSize);
        List<TitleDto> dtos = new();
        foreach (var title in titles)
        {
            var dto = Mapper.Map<TitleDto>(title);
            dto.Episodes = title.TitleType.Equals("tvSeries")
                ? GetUrl(nameof(TitlesController.GetEpisodesOfSeries), new { tconst = title.Tconst.Trim() })
                : null;
            dto.Aliases = GetUrl(nameof(TitlesController.GetTitleAliases), new { tconst = title.Tconst.Trim() });
            dto.Url = GetUrl(nameof(TitlesController.GetTitle), new { tconst = title.Tconst.Trim() });
            dto.Genres = Mapper.Map<List<GenreDto>>(title.Genre);
            dto.Crew = GetUrl(nameof(CrewController.GetCrew), new { tconst = title.Tconst.Trim() });
            dtos.Add(dto);
        }

        return Ok(Paging(dtos, total, new NconstPagingValues {Nconst = nconst, Page = page, PageSize = pageSize },
            nameof(GetTitlesFromPerson)));
    }

    private PersonDto MapPerson(Person person)
    {
        var dto = Mapper.Map<PersonDto>(person);
        dto.Url = GetUrl(nameof(GetPerson), new { nconst = person.Nconst.Trim() });
        dto.Professions = Mapper.Map<List<ProfessionDto>>(person.Professions);
        return dto;
    }
    
    private class NconstPagingValues : PagingValues
    {
        public string Nconst { get; set; }
    }
}