using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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

        public List<Rating> GetRatings(int userId)
        {
            var db = new MovieDbContext();
            return db.Rated.Where(x => x.Id == userId).ToList();
        }

        public Rating? GetRating(string tconst)
        {
            var db = new MovieDbContext();
            return db.Rated.FirstOrDefault(x => x.Tconst==tconst);
        }

        
        public bool UpdateRating(int userId, string tconst, int updatedRating)
        {

            var db = new MovieDbContext();  
            db.Database.ExecuteSqlRaw($"call update_rating({userId}, '{tconst}', {updatedRating})");
            return db.SaveChanges() > 0;

        }

        
        public void  DeleteRating(int userId, string tconst)

        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call remove_rating({userId}, '{tconst}')");
            db.SaveChanges();
        }
    }
}

