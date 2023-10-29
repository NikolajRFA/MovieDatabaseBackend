using DataLayer.DataServices;
using DataLayer.DbSets;
using Xunit.Abstractions;

namespace DataLayerTests.DataServiceTests;

public class UserDataServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UserDataServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetUsers_10UsersOnAPage_GetsFirst10Users()
    {
        var dataService = new UserDataService();
        var users = dataService.GetUsers();
        _testOutputHelper.WriteLine($"Total users {users.count}");
        // ensure traversability
        users.users.ForEach(user =>
        {
            _testOutputHelper.WriteLine($"user: {user.Username} has {user.Searches.Count} searches.");
        });
        Assert.NotNull(users);
    }

    [Fact]
    public void GetUser_UserId2_GetsNikoUser()
    {
        var dataService = new UserDataService();
        var user = dataService.GetUser(2);
        _testOutputHelper.WriteLine($"Username: {user.Username}");
        Assert.Equal("Niko", user.Username);
    }

    [Fact]
    public void GetUser_InvalidUserId_GetsNullUser()
    {
        var dataService = new UserDataService();
        var user = dataService.GetUser(9999999);
        Assert.Null(user);
    }
}