using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DbSets
{
    public class Rating
    {
        public string Tconst { get; set;}
        public Title Title { get; set; }
        public int Id { get; set;}
        public User User { get; set; }
        public int ThisRating { get; set;}
        public DateTime Date { get; set;}
    }
}
