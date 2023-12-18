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

    [HttpGet("results", Name = nameof(GetPersonsWithName))]
    public IActionResult GetPersonsWithName(string q, int page = 0, int pageSize = 10)
    {
        var (persons, total) = _dataService.GetPersonsWithName(q, page, pageSize);
        List<PersonDto> dtos = new();
        foreach (var person in persons)
        {
            dtos.Add(MapPerson(person));
        }
        return Ok(Paging(dtos, total, new PagingValues { Page = page, PageSize = pageSize }, nameof(GetPersonsWithName)));
    }

    [HttpGet("{nconst}/titles", Name = nameof(GetTitlesFromPerson))]
    public IActionResult GetTitlesFromPerson(string nconst, int page = 0, int pageSize = 10)
    {
        var (titles, total) = _dataService.GetTitlesFromPerson(nconst, page, pageSize);
        List<TitleDto> dtos = new();
        foreach (var title in titles)
        {
            dtos.Add(MapTitle(title));
        }

        return Ok(Paging(dtos, total, new NconstPagingValues {Nconst = nconst, Page = page, PageSize = pageSize },
            nameof(GetTitlesFromPerson)));
    }

    private PersonDto MapPerson(Person person)
    {
        var dto = Mapper.Map<PersonDto>(person);
        dto.Url = GetUrl(nameof(GetPerson), new { nconst = person.Nconst.Trim() });
        dto.Professions = Mapper.Map<List<ProfessionDto>>(person.Professions);
        dto.TitlesUrl = GetUrl(nameof(GetTitlesFromPerson), new { nconst = person.Nconst.Trim() });
        return dto;
    }
    
    private class NconstPagingValues : PagingValues
    {
        public string Nconst { get; set; }
    }
}