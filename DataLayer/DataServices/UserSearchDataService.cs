using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DbSets;

namespace DataLayer.DataServices
{
    public class UserSearchDataService
    {
        public Search? GetSearch(string search)
        {
            var db = new MovieDbContext();
            return db.Searches.OrderByDescending(x=>x.Date).FirstOrDefault(x => x.SearchPhrase.Equals(search));
        }

        public (List<Search> searches, int count) GetSearches(int userId, int page, int pageSize)
        {
            var db = new MovieDbContext();
            var count = db.Searches.Count(x => x.Id == userId);
            var searches = db.Searches
                .Where(x=>x.Id == userId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (searches, count);
        }
    }
}
