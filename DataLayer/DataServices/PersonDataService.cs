﻿using DataLayer.DbSets;
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
            .Select(word => !string.IsNullOrEmpty(word) ? char.ToUpper(word[0]) + word.Substring(1) : string.Empty)
            .ToArray();

        var capitalizedName = string.Join(" ", searchPhrases);

        var db = new MovieDbContext();
        var personsWithTotals = db.PersonsWithTotals
            .FromSqlRaw($"select * from string_search_person('{capitalizedName}', {page}, {pageSize})")
            .ToList();
        var persons = new List<Person>();
        foreach (var personWithTotal in personsWithTotals)
        {
            persons.Add(new Person
            {
                Nconst = personWithTotal.Nconst,
                Name = personWithTotal.Name,
                BirthYear = personWithTotal.BirthYear,
                DeathYear = personWithTotal.DeathYear,
                NameRating = personWithTotal.NameRating
            });
        }

        var total = personsWithTotals.FirstOrDefault() != null ? personsWithTotals.FirstOrDefault()!.Total : 0;

        return (persons, total);
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