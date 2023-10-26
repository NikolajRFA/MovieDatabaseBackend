using DataLayer;
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
}