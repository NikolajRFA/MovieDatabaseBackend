﻿using DataLayer.DbSets;
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
        return db.Bookmarks
            .Include(x => x.Person)
            .Include(x => x.Title)
            .Where(x => x.UserId == userId).ToList();
    }

    public Bookmark? GetBookmark(int id)
    {
        var db = new MovieDbContext();
        return db.Bookmarks.FirstOrDefault(x => x.Id == id);
    }
        
    public void DeleteBookmark(int id)
    {
        var db = new MovieDbContext();
        var bookmark = db.Bookmarks.FirstOrDefault(x => x.Id == id);
        if(bookmark == null) return;
        db.Bookmarks.Remove(bookmark);
        db.SaveChanges();
    }

    public Bookmark? GetTitleBookmark(int userId, string id)
    {
        var db = new MovieDbContext();
        return db.Bookmarks
            .Include(x => x.Title)
            .FirstOrDefault(x => x.UserId == userId && x.Tconst == id);
    }

    public Bookmark? GetPersonBookmark(int userId, string id)
    {
        var db = new MovieDbContext();
        return db.Bookmarks
            .Include(x => x.Person)
            .FirstOrDefault(x => x.UserId == userId && x.Nconst.Equals(id));
    }
}