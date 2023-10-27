namespace DataLayer.DbSets;

public class Person
{
    public string Nconst { get; set; }
    public string PersonName { get; set; }
    public int BirthYear { get; set; }
    public int DeathYear { get; set; }
    public double NameRating { get; set; }
    public List<Profession> Profession { get; set; }
}