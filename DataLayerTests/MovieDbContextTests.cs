using System.Runtime.InteropServices.ComTypes;
using DataLayer;
using DataLayer.DataServices;
using Microsoft.EntityFrameworkCore;

namespace DataLayerTests;

public class MovieDbContextTests
{
    [Fact]
    public void ConnectToDb_NoActions_DoesNotFail()
    {
        var db = new MovieDbContext();
    }

    [Fact]
    public void GetTitles_Top10Titles_Gets10Titles()
    {
        var db = new MovieDbContext();
        Assert.Equal(10, db.Titles.Take(10).Count());
    }

    [Fact]
    public void GetPersons_Top10Persons_Gets10Persons()
    {
        var db = new MovieDbContext();
        Assert.Equal(10, db.Persons.Take(10).Count());
    }

    [Fact]
    public void GetCrew_Top10Crew_Gets10Crew()
    {
        var db = new MovieDbContext();
        var count = db.Crew
            .Include(x => x.Person)
            .Include(x => x.Title)
            .Take(10)
            .Count();
        Assert.Equal(10, count);
    }

    [Fact]
    public void GetAliases_Top10Aliases_Gets10Aliases()
    {
        var db = new MovieDbContext();
        var count = db.Aliases
            .Include(x => x.ThisTitle)
            .Take(10)
            .Count();
        Assert.Equal(10, count);
    }

    [Fact]
    public void GetIsEpisodeOf_Top10Items_Gets10Items()
    {
        var db = new MovieDbContext();
        var count = db.IsEpisodeOf
            .Include(x => x.Title).Include(x => x.ParentTitle)
            .Take(10)
            .Count();
        Assert.Equal(10, count);
    }

    [Fact]
    public void GetGenres_Top10Genres_Gets10Genres()
    {
        var db = new MovieDbContext();
        var count = db.Genres.Include(x => x.Title).Take(10).Count();
        Assert.Equal(10, count);
    }

    [Fact]
    public void GetProfessions_Top10Professions_Gets10Professions()
    {
        var db = new MovieDbContext();
        var count = db.Professions.Include(x => x.Person).Take(10).Count();
        Assert.Equal(10, count);
    }

    [Fact]
    public void GetWi_Top10Wi_Gets10Wi()
    {
        var db = new MovieDbContext();
        var count = db.Wi.Take(10).Count();
        Assert.Equal(10, count);
    }

    [Fact] // Will fail if no users are created
    public void GetUser_GetFirstUser_Gets1User()
    {
        var db = new MovieDbContext();
        var count = db.Users.Take(1).Count();
        Assert.Equal(1, count);
    }

    [Fact]
    public void GetFirstSearchFromUser_UserNiko_GetsStarWarsSearch()
    {
        var db = new MovieDbContext();
        string[] searches = new[] { "star", "wars" };
        db.Database.ExecuteSqlRaw($"select * from best_match(35, 0, 10, '${searches[0]}', '${searches[1]}')");
        db.SaveChanges();
        var firstSearchPhrase = db.Users
            .Include(x => x.Searches)
            .First(x => x.Username.Equals("Niko"))
            .Searches
            .OrderBy(x => x.Date)
            .First()
            .SearchPhrase;
        Assert.Equal(string.Join(",", searches), firstSearchPhrase);
    }

    [Fact]
    public void GetFirstRatingFromUser_UserNiko_GetRating()
    {
        var db = new MovieDbContext();
        const int rating = 10;
        db.Database.ExecuteSqlRaw("DELETE FROM rated where id = 35 AND tconst = 'tt10850402'");
        db.Database.ExecuteSqlRaw($"call rate_title(35, 'tt10850402', {rating})");
        var firstRating = db.Users
            .Include(x => x.Ratings)
            .First(x => x.Username.Equals("Niko")).Ratings
            .OrderBy(x => x.Date).First().ThisRating;
        Assert.Equal(rating, firstRating);
    }

    [Fact]
    public void GetFirstBookmarkFromUser_UserNiko_GetBookmark()
    {
        var db = new MovieDbContext();
        var firstBookmark = db.Users
            .Include(x => x.Bookmarks)
            .First(x => x.Username.Equals("Niko")).Bookmarks
            .OrderBy(x => x.Id).First();
        Assert.Equal("nm0000434", firstBookmark.Nconst.Trim());
    }
}