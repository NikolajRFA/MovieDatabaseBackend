namespace DataLayer.DbSets;

public class Person
{
    public string Nconst { get; set; }
    public string Name { get; set; }
    public string BirthYear { get; set; }
    public string DeathYear { get; set; }
    public double NameRating { get; set; }
    public List<Profession> Professions { get; set; }
    public List<Crew> Crews { get; set; }
}