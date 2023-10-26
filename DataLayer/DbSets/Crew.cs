namespace DataLayer.DbSets;

public class Crew
{
    public int Id { get; set; }
    public string Tconst { get; set; }
    public int Ordering { get; set; }
    public string Nconst { get; set; }
    public string Category { get; set; }
    public string Job { get; set; } 
    public string Character { get; set; }
    public Title Title { get; set; }
    public Person Person { get; set; }
}