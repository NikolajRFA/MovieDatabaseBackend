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
        var count = db.Users.Count();
        return (users, count);
    }

    public User? CreateUser(string username, string email, string password)
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call create_user('{username}', '{email}', '{password}')");
        var user = db.Users.SingleOrDefault(x => x.Username.Equals(username));
        return user;
    }

    public bool DeleteUser(int id)
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call delete_user({id})");
        return !db.Users.Any(x => x.Id == id);
    }
}