using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DbSets;
using Microsoft.AspNetCore.Mvc;
using WebServer.Controllers;
using WebServer.DataTransferObjects;


namespace WebServer.Controllers;

[Route("api/users/{userId:int}/searches")]
[ApiController]


    public class UserSearchController : GenericControllerBase
    {
        private readonly UserSearchDataService _dataService;

        public UserSearchController(UserSearchDataService dataService, LinkGenerator linkGenerator, IMapper mapper) :
            base(
                linkGenerator, mapper)
        {
            _dataService = dataService;

            
        }

        [HttpGet(nameof(GetSearch))]
        public ActionResult GetSearch(string searchPhrase)

        {
            var search = _dataService.GetSearch(searchPhrase);
            if (search == null) return NotFound();
            var dto = new SearchDto
            { 
                User = GetUrl(nameof(UsersController.GetUser), new {search.Id }),
                SearchPhrase = search.SearchPhrase,
                Date = search.Date,


            };
            return Ok(dto);
        }
    }

