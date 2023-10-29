using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
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
}