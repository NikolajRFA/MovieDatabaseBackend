using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : GenericControllerBase
{
    private readonly UserDataService _dataService;

    public UserController(UserDataService dataService, LinkGenerator linkGenerator, IMapper mapper) : base(
        linkGenerator, mapper)
    {
        _dataService = dataService;
    }
}