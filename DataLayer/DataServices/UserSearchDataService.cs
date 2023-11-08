using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class UserSearchDataService
    {
        public Search? GetSearch(int userId, string search)
        {
            var db = new MovieDbContext();
            return db.Searches.OrderByDescending(x => x.Date)
                .FirstOrDefault(x => x.SearchPhrase.Equals(search) && x.Id == userId);
        }

        public (List<Search> searches, int count) GetSearches(int userId, int page, int pageSize)
        {
            var db = new MovieDbContext();
            var count = db.Searches.Count(x => x.Id == userId);
            var searches = db.Searches
                .Where(x => x.Id == userId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (searches, count);
        }

        public void DeleteSearch(int userId, string search)
        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call remove_search({userId}, '{search}')");
            db.SaveChanges();
        }

        public void DeleteSearches(int userId)
        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call remove_searches({userId})");
            db.SaveChanges();
        }
    }
}