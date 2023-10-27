using System.Diagnostics.CodeAnalysis;
using DataLayer.DbSets;

namespace DataLayer.DataServices;

public class TitleDataService
{
    public (List<Title> titles, int count) GetTitles(int page, int pageSize)
    {
        var db = new MovieDbContext();
        var count = db.Titles.Count();
        var titles =
            db.Titles
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
}