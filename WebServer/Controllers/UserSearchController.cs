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

        [HttpDelete("{searchPhrase}")]
        public IActionResult DeleteSearch(int userId, string searchPhrase)
        {
            _dataService.DeleteSearch(userId, searchPhrase);
            return Ok();
        }

        [HttpDelete("clear")]
        public IActionResult DeleteSearches(int userId)
        {
            _dataService.DeleteSearches(userId);
            return Ok();
        }

        [HttpGet(Name = nameof(GetSearches))]
        public IActionResult GetSearches(int userId, int page = 0, int pageSize = 10)
        {
            var (searches, count) = _dataService.GetSearches(userId, page, pageSize);
            List<SearchDto> searchDtos = new List<SearchDto>();
            foreach (var search in searches)
            {
                var dto = MapSearch(search);
                searchDtos.Add(dto);
            } 
        return Ok(Paging(searchDtos, count, new 
                UserSearchPagingValues{ UserId = userId, Page = page, PageSize = pageSize }, nameof(GetSearches)));
        }


        [HttpGet("{searchPhrase}", Name=nameof(GetSearch))]
        public ActionResult GetSearch(int userId, string searchPhrase)

        {
            var search = _dataService.GetSearch(userId, searchPhrase);
            if (search == null) return NotFound();
            var dto = new SearchDto
            { 
                User = GetUrl(nameof(UsersController.GetUser), new {search.Id }),
                SearchPhrase = search.SearchPhrase,
                Date = search.Date,
            };
            return Ok(dto);
        }

        private SearchDto MapSearch(Search search)
        {
            var searchDto = Mapper.Map<SearchDto>(search);
            searchDto.User = GetUrl(nameof(UsersController.GetUser), new { search.Id });
            searchDto.SearchPhrase = search.SearchPhrase;
            searchDto.Date = search.Date;
            return searchDto;
        }

        private class UserSearchPagingValues : PagingValues
        {
            public int UserId { get; set; }
        }
}



