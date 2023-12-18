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

    public (List<Person>, int) GetPersonsWithName(string name, int page = 0, int pageSize = 10)
    {
        var searchPhrases = name.Split(" ")
            .Select(word => char.ToUpper(word[0]) + word.Substring(1))
            .ToArray();
    
        var capitalizedName = string.Join(" ", searchPhrases);
        
        var db = new MovieDbContext();
        var persons = db.Persons
            .FromSqlRaw($"select * from string_search_person('{capitalizedName}')")
            .ToList();
        return (persons, persons.Count());
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

    public (List<Title>, int) GetTitlesFromPerson(string nconst, int page = 0, int pageSize = 10)
    {
        var db = new MovieDbContext();
        var titles = db.Titles
            .Include(x => x.Crew)
            .Include(x => x.Genre)
            .Where(x => x.Crew.Any(y => y.Nconst.Equals(nconst.Trim())))
            .OrderByDescending(x => x.NumVotes);

        var pagedTitles = titles
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        var total = titles.Count();
        return (pagedTitles, total);
    }
}