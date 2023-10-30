using Microsoft.AspNetCore.Mvc;
using System.Collections;
using AutoMapper;

namespace WebServer.Controllers;

public class GenericControllerBase : ControllerBase
{
    protected IMapper Mapper { get; }
    private readonly LinkGenerator _linkGenerator;

    public GenericControllerBase(LinkGenerator linkGenerator, IMapper mapper)
    {
        Mapper = mapper;
        _linkGenerator = linkGenerator;
    }

    protected object Paging<T>(IEnumerable<T> items, int total, PagingValues pagingValues, string endpointName)
    {
        var nextPagingValues = (PagingValues) pagingValues.Clone();
        nextPagingValues.Page += 1;
        var prevPagingValues = (PagingValues) pagingValues.Clone();
        prevPagingValues.Page -=  1;
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

