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
        public void CreateRating(int userId, string tconst, int rating)
        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call rate_title({userId},'{tconst}', {rating})");
            db.SaveChanges();
        }

        public (List<Rating> ratings, int count) GetRatings(int userId, int page, int pageSize)
        {
            var db = new MovieDbContext();
            var count = db.Rated.Count(x => x.Id == userId);
            var ratings = db.Rated
                .Include(x => x.Title)
                .Where(x => x.Id == userId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (ratings, count);
        }

        public Rating? GetRating(string tconst, int userId)
        {
            var db = new MovieDbContext();
            return db.Rated
                .Include(x => x.Title)
                .FirstOrDefault(x => x.Tconst.Equals(tconst) && x.User.Id == userId);
        }


        public bool UpdateRating(int userId, string tconst, int updatedRating)
        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call update_rating({userId}, '{tconst}', {updatedRating})");
            return db.SaveChanges() > 0;
        }


        public void DeleteRating(int userId, string tconst)

        {
            var db = new MovieDbContext();
            db.Database.ExecuteSqlRaw($"call remove_rating({userId}, '{tconst}')");
            db.SaveChanges();
        }
    }
}