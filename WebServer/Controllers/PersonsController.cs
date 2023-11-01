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

    private PersonDto MapPerson(Person person)
    {
        var dto = Mapper.Map<PersonDto>(person);
        dto.Url = GetUrl(nameof(GetPerson), new { nconst = person.Nconst.Trim() });
        dto.Professions = Mapper.Map<List<ProfessionDto>>(person.Professions);
        return dto;
    }
}