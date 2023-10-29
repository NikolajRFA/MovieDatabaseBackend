using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class RatingDataService
    {
        public void CreateRating(int userId, string tconst, double rating)
        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call rate_title({userId},'{tconst}', {rating})");
            db.SaveChanges();
        }

        public List<Rating> GetRating(int userId)
        {
            var db = new MovieDbContext();
            return db.Rated.Where(x => x.Id == userId).ToList();
        }

        public void UpdateRating(int userId, double rating)
        {

        }


        public void DeleteRating(int userId, string tconst)

        {

        }

    }
}
