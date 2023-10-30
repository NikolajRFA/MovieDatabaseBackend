using AutoMapper;
using DataLayer.DataServices;
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

        var dto = Mapper.Map<PersonDto>(person);
        dto.Url = GetUrl(nameof(GetPerson), new { nconst });
        dto.Professions = Mapper.Map<List<ProfessionDto>>(person.Professions);
        return Ok(dto);
    }
}