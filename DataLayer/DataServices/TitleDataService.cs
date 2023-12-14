using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
        var title = db.Titles
            .Include(x => x.Genre)
            .FirstOrDefault(x => x.Tconst == tconst);
        return title;
    }

    public (List<IsEpisodeOf>?, int) GetEpisodes(string tconst, int page = 0, int pageSize = 10, int season = 0)
    {
        Console.WriteLine($"season: {season}");
        var db = new MovieDbContext();
        // If title is not a 'tvSeries'
        if (!db.Titles.First(x => x.Tconst.Equals(tconst.Trim())).TitleType.Equals("tvSeries")) return (null, 0);
        var episodes = db.IsEpisodeOf
            .Include(x => x.Title)
            .Where(x => x.ParentTconst.Equals(tconst.Trim()))
            .Where(x => season == 0 || x.SeasonNumber == season)
            .OrderBy(x => x.SeasonNumber)
            .ThenBy(x => x.EpisodeNumber);

        return (episodes.Skip(page * pageSize).Take(pageSize).ToList(), episodes.Count());
    }

    public (List<Title> titles, int count) GetTitlesSearch(int? id, string q, int page, int pageSize)
    {
        var db = new MovieDbContext();
        var (results, count) = BestMatchSearch(db, q, page, pageSize, id);
        List<Title> titles = new();
        foreach (var bestMatch in results)
        {
            titles.Add(db.Titles
                    .Include(x => x.Crew.OrderBy(x => x.Ordering))
                    .ThenInclude(x => x.Person)
                    .Include(x => x.Genre)
                    .FirstOrDefault(x =>
                        x.Tconst.Equals(bestMatch.Tconst.Trim()))!
            );
        }

        return (titles, count);
    }

    public (List<Title> titles, int count) GetTitlesSearchForDropdown(string q, int dropdownSize)
    {
        var db = new MovieDbContext();
        var (results, count) = BestMatchSearch(db, q, 0, dropdownSize);
        List<Title> titles = new();
        foreach (var bestMatch in results)
        {
            titles.Add(db.Titles
                    .Include(x => x.Crew.OrderBy(x => x.Ordering).Take(2))
                    .ThenInclude(x => x.Person)
                    .Include(x => x.Genre)
                    .FirstOrDefault(x =>
                        x.Tconst.Equals(bestMatch.Tconst.Trim()))!
            );
        }

        return (titles, count);
    }

    private (List<BestMatch>, int) BestMatchSearch(MovieDbContext db, string search, int page, int pageSize,
        int? userId = null)
    {
        var qStrings = search.Split(" ");
        // Sanitize input
        qStrings = qStrings.Select(x => Regex.Replace(x, @"'", "''")).ToArray();
        var variadic = string.Join("', '", qStrings);
        // Build the search function call.
        // If userId is null the overload of best_match_field that doesn't take a user id is used.
        var bestMatchCall = $"best_match({(userId == null ? "" : $"{userId},")} {page}, {pageSize}, '{variadic}')";

        var bestMatches = db.BestMatches.FromSqlRaw($"SELECT * FROM {bestMatchCall}").ToList();

        return (bestMatches, bestMatches.FirstOrDefault(new BestMatch { Total = 0 }).Total);
    }

    public (List<Alias>, int) GetTitleAliases(string tconst, int page, int pageSize)
    {
        var db = new MovieDbContext();
        var aliases = db.Aliases
            .Where(x => x.Tconst.Equals(tconst.Trim()))
            .OrderBy(x => x.Ordering);

        return (aliases
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList(),
                aliases.Count()
            );
    }
}