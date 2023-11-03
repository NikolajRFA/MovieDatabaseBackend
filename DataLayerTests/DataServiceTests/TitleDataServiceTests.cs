using DataLayer.DataServices;

namespace DataLayerTests.DataServiceTests;

public class TitleDataServiceTests
{
    [Fact]
    public void GetBestMatches_StarWarsRevengeSith_ReturnsTt0121766()
    {
        var dataService = new TitleDataService();
        var (matches, total) = 
            dataService.BestMatchSearch("Star wars revenge sith", 't', 2);
        Assert.Contains(matches, x => x.Tconst.Trim().Equals("tt0121766"));
        Assert.Equal(1, total);
    }
    
    [Fact]
    public void GetBestMatches_Office_Returns43Results()
    {
        var dataService = new TitleDataService();
        var (matches, total) =
            dataService.BestMatchSearch("Office", 't', 2);
        Assert.Equal(43, total);
    }
}