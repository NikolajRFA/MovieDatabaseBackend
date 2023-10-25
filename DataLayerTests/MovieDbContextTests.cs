using DataLayer;

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
}