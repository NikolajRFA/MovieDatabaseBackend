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

    public (List<Person>, int) GetPersons(int page = 0, int pageSize = 10)
    {
        var db = new MovieDbContext();
        var persons = db.Persons
            .Include(x => x.Professions)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        var total = db.Persons.Count();
        return (persons, total);
    }
}