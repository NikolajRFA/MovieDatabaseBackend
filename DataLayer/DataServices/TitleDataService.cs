using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices;

public class TitleDataService
{
    public (List<Title> titles, int count) GetTitles(int page, int pageSize)
    {
        var db = new MovieDbContext();
        var count = db.Titles.Count();
        var titles =
            db.Titles
                .Include(x => x.Genre)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        return (titles, count);
    }

    public Title GetTitle(string tconst)
    {
        var db = new MovieDbContext();
        var title = db.Titles.FirstOrDefault(x => x.Tconst == tconst);
        return title;
    }

    public (List<Title> titles, int count) GetTitlesSearch(int id, string q, int page, int pageSize)
    {
        var qStrings = q.Split(" ");
        qStrings = qStrings.Select(x => Regex.Replace(x, @"'", "''")).ToArray();
        var variadicString = string.Join("', '", qStrings);
        var db = new MovieDbContext();
        var results = db.BestMatches.FromSqlRaw($"SELECT * FROM best_match({id}, '{variadicString}')");
        var filterResults = results.Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        List<Title> titles = new();
        foreach (var bestMatch in filterResults)
        {
            titles.Add(db.Titles
                .Include(x => x.Genre)
                .FirstOrDefault(x => 
                    x.Tconst.Trim().Equals(bestMatch.Tconst.Trim()))!
                );
        }

        var count = results.Count();
        return (titles, count);
    }

    public (List<Title> titles, int count) GetTitlesSearchForDropdown(string q, int dropdownSize)
    {
        var qStrings = q.Split(" ");
        qStrings = qStrings.Select(x => Regex.Replace(x, @"'", "''")).ToArray();
        var variadicString = string.Join("', '", qStrings);
        var db = new MovieDbContext();
        var results = db.BestMatches.FromSqlRaw($"SELECT * FROM best_match('{variadicString}')");
        var filterResults = results.Take(dropdownSize)
            .ToList();
        List<Title> titles = new();
        foreach (var bestMatch in filterResults)
        {
            titles.Add(db.Titles
                    .Include(x => x.Crew.OrderBy(x=>x.Ordering).Take(2))
                    .ThenInclude(x => x.Person)
                    .FirstOrDefault(x => 
                        x.Tconst.Trim().Equals(bestMatch.Tconst.Trim()))!
            );
        }

        var count = results.Count();
        return (titles, count);
    }
}