using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices;

public class BookmarkDataService
{
    public void CreateMovieBookmark(int userId, string tconst)
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call bookmark({userId}, '{tconst}', null)");
        db.SaveChanges();
    }
    public void CreatePersonBookmark(int userId, string nconst)
    {
        var db = new MovieDbContext();
        db.Database.ExecuteSqlRaw($"call bookmark({userId}, null, '{nconst}')");
        db.SaveChanges();
    }

    public List<Bookmark> GetBookmarks(int userId)
    {
        var db = new MovieDbContext();
        return db.Bookmarks.Where(x => x.UserId == userId).ToList();
    }

    public void DeleteBookmark(int id)
    {
        var db = new MovieDbContext();
        var bookmark = db.Bookmarks.FirstOrDefault(x => x.Id == id);
        if(bookmark == null) return;
        db.Bookmarks.Remove(bookmark);
        db.SaveChanges();
    }
}