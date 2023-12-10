using DataLayer.DataServices;
using Xunit.Abstractions;

namespace DataLayerTests.DataServiceTests;

public class PersonDataServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonDataServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetTitlesFromPerson_ValidInput_Success()
    {
        var dataService = new PersonDataService();
        var (matches, total) = 
            dataService.GetTitlesFromPerson("nm0136797");
        _testOutputHelper.WriteLine("");
    }
}