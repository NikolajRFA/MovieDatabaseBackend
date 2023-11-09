using DataLayer.DataServices;

namespace DataLayerTests.DataServiceTests;

public class TitleDataServiceTests
{
    [Fact]
    public void GetBestMatches_StarWarsRevengeSith_ReturnsTt0121766()
    {
        var dataService = new TitleDataService();
        var (matches, total) = 
            dataService.GetTitlesSearch(1,"star wars revenge sith", 0, 10);
        Assert.Contains(matches, x => x.Tconst.Trim().Equals("tt0121766"));
        Assert.Equal(33, total);
    }
    
    [Fact]
    public void GetBestMatches_Office_Returns43Results()
    {
        var dataService = new TitleDataService();
        var (matches, total) =
            dataService.GetTitlesSearch(1,"office", 0, 10);
        Assert.Equal(588, total);
    }
}