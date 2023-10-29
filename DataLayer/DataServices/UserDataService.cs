using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices;

public class UserDataService
{
    public User? GetUser(int id)
    {
        var db = new MovieDbContext();
        var user = db.Users
            .Include(x => x.Searches)
            .Include(x => x.Bookmarks)
            .Include(x => x.Ratings)
            .SingleOrDefault(x => x.Id == id);
        return user;
    }

    public (List<User> users, int count) GetUsers(int page = 0, int pageSize = 10)
    {
        var db = new MovieDbContext();
        var users = db.Users
            .Include(x => x.Searches)
            .Include(x => x.Bookmarks)
            .Include(x => x.Ratings)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        var count = users.Count;
        return (users, count);
    }
}