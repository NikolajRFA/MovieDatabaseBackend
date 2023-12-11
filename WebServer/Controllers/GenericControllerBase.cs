using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;
using AutoMapper;
using DataLayer.DbSets;
using WebServer.DataTransferObjects;

namespace WebServer.Controllers;

public abstract class GenericControllerBase : ControllerBase
{
    protected IMapper Mapper { get; }
    private readonly LinkGenerator _linkGenerator;
    protected int? UserId => ExtractUserIdFromClaim();

    public GenericControllerBase(LinkGenerator linkGenerator, IMapper mapper)
    {
        Mapper = mapper;
        _linkGenerator = linkGenerator;
    }

    protected object Paging<T>(IEnumerable<T> items, int total, PagingValues pagingValues, string endpointName)
    {
        var nextPagingValues = (PagingValues)pagingValues.Clone();
        nextPagingValues.Page += 1;
        var prevPagingValues = (PagingValues)pagingValues.Clone();
        prevPagingValues.Page -= 1;
        
        var numPages = (int)Math.Ceiling(total / (double)pagingValues.PageSize);
        var next = pagingValues.Page < numPages - 1
            ? GetUrl(endpointName, nextPagingValues)
            : null;
        var prev = pagingValues.Page > 0
            ? GetUrl(endpointName, prevPagingValues)
            : null;

        var cur = GetUrl(endpointName, pagingValues);

        return new
        {
            Total = total,
            NumberOfPages = numPages,
            Next = next,
            Prev = prev,
            Current = cur,
            Items = items
        };
    }

    protected string GetUrl(string name, object values)
    {
        return _linkGenerator.GetUriByName(HttpContext, name, values) ?? "Not specified";
    }
    
    protected int? ExtractUserIdFromClaim()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return null;
        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return null;
        }

        return userId;
    }
    
    protected TitleDto MapTitle(Title title)
    {
        var dto = Mapper.Map<TitleDto>(title);
        dto.Episodes = title.TitleType.Equals("tvSeries")
            ? GetUrl(nameof(TitlesController.GetEpisodesOfSeries), new { tconst = title.Tconst.Trim() })
            : null;
        dto.Aliases = GetUrl(nameof(TitlesController.GetTitleAliases), new { tconst = title.Tconst.Trim() });
        dto.Url = GetUrl(nameof(TitlesController.GetTitle), new { tconst = title.Tconst.Trim() });
        dto.Genres = Mapper.Map<List<GenreDto>>(title.Genre);
        dto.Crew = GetUrl(nameof(CrewController.GetCrew), new { tconst = title.Tconst.Trim() });
        return dto;
    }
}

public class PagingValues : ICloneable
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
