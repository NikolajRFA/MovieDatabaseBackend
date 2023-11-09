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
        Assert.NotNull(users.users);
    }

    [Fact]
    public void GetUser_UserId2_GetsNikoUser()
    {
        var dataService = new UserDataService();
        var user = dataService.GetUser(35);
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

    [Fact]
    public void CreateUser_NewUser_ReturnsNewUser()
    {
        var dataService = new UserDataService();
        var newUser = new
        {
            Username = "new_user",
            Email = "new_user@email.com",
            Password = "1234",
            Salt = "12341234"
        };
        var createdNewUser = dataService.CreateUser(
            newUser.Username, newUser.Email, newUser.Password, newUser.Salt
        );
        Assert.NotNull(dataService.GetUser(createdNewUser.Id));
        dataService.DeleteUser(createdNewUser.Id);
    }

    [Fact]
    public void UpdateUser_NewUser_UpdatesNewUser()
    {
        var dataService = new UserDataService();
        var newUser = new
        {
            Username = "new_user",
            Email = "new_user@email.com",
            Password = "1234",
            Salt = "12341234"
        };
        var createdNewUser = dataService.CreateUser(
            newUser.Username, newUser.Email, newUser.Password, newUser.Salt
        );
        var updatedUser = dataService.UpdateUser(
            createdNewUser.Id,
            "updated_username",
            "updated@email.com",
            createdNewUser.Password,
            createdNewUser.Salt,
            createdNewUser.Role
        );
        Assert.Equal("updated_username", dataService.GetUser(createdNewUser.Id)!.Username);
        dataService.DeleteUser(createdNewUser.Id);
    }

    [Fact]
    public void DeleteUser_NewUser_DeletesNewUser()
    {
        var dataService = new UserDataService();
        var newUser = new
        {
            Username = "new_user",
            Email = "new_user@email.com",
            Password = "1234",
            Salt = "12341234"
        };
        var createdNewUser = dataService.CreateUser(
            newUser.Username, newUser.Email, newUser.Password, newUser.Salt
        );
        Assert.NotNull(dataService.GetUser(createdNewUser.Id));
        dataService.DeleteUser(createdNewUser.Id);
        Assert.Null(dataService.GetUser(createdNewUser.Id));
    }
}