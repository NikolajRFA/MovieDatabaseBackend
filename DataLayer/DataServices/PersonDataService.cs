using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices;

public class PersonDataService
{
    public Person? GetPerson(string nconst)
    {
        var db = new MovieDbContext();
        return db.Persons
            .Include(x => x.Professions)
            .SingleOrDefault(x => x.Nconst.Equals(nconst));
    }
}