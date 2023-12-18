using DataLayer.DbSets;
using EFCore.NamingConventions.Internal;
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

    public User? GetUser(string name)
    {
        var db = new MovieDbContext();
        var user = db.Users
            .Include(x => x.Searches)
            .Include(x => x.Bookmarks)
            .Include(x => x.Ratings)
            .SingleOrDefault(x => x.Username == name);
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

    public User? CreateUser(string username, string email, string password, string salt, string role = "User")
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call create_user('{username}', '{email}', '{password}', '{salt}', '{role}')");
        var user = db.Users.SingleOrDefault(x => x.Email.Equals(email));
        return user;
    }

    public bool DeleteUser(int id)
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call delete_user({id})");
        return !db.Users.Any(x => x.Id == id);
    }

    public User? UpdateUser(int id, string username, string email, string? hashedPassword, string? salt, string role)
    {
        var db = new MovieDbContext();
        if (hashedPassword == null)
        {
            var thisUser = db.Users.SingleOrDefault(x => x.Id == id);
            hashedPassword = thisUser?.Password;
            salt = thisUser?.Salt;
        }
        
        db.Database.ExecuteSqlRaw($"call update_user({id}, '{username}', '{email}', '{hashedPassword}', '{salt}', '{role}')");
        var user = db.Users
            .Include(x => x.Searches)
            .Include(x => x.Bookmarks)
            .Include(x => x.Ratings)
            .SingleOrDefault(x => x.Id.Equals(id));
        
        if (db.Users.Any(x => x.Email.Equals(email) && x.Id != id)) return null;
        return user;
    }
}