using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices;

public class CrewDataService
{
    /// <summary>
    /// Gets the crew of a title.
    /// </summary>
    /// <param name="tconst">the tconst of the title.</param>
    /// <returns>List of the crew involved in the title.</returns>
    public (List<Crew>, int) GetCrew(string tconst, int page, int pageSize)
    {
        var db = new MovieDbContext();
        var crew = db.Crew
            .Include(x => x.Person)
            .Where(x => x.Tconst.Equals(tconst.Trim()));
        
        var crewTake = crew
            .OrderBy(x => x.Ordering)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        var total = crew.Count();

        return (crewTake, total);
    }

    public Crew? GetCrewSingle(int id)
    {
        var db = new MovieDbContext();
        var crew = db.Crew
            .Include(x => x.Person)
            .SingleOrDefault(x => x.Id == id);

        return crew;
    }
}