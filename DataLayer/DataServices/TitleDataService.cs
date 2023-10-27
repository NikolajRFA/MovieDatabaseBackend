using DataLayer.DbSets;

namespace DataLayer.DataServices;

public class TitleDataService
{
    public (List<Title> titles, int count) GetTitles(int page, int pageSize)
    {
        var db = new MovieDbContext();
        var titles =
            db.Titles
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        return (titles, db.Titles.ToList().Count);
    }

    public Title GetTitle(string tconst)
    {
        var db = new MovieDbContext();
        var title = db.Titles.FirstOrDefault(x => x.Tconst == tconst);
        return title;
    }
}