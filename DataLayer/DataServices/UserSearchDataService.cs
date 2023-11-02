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
    }
}
