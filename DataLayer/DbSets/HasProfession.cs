namespace DataLayer.DbSets;

public class HasProfession
{
    public string Nconst { get; set; }
    public int ProfessionId { get; set; }
    public Person Person { get; set; }
    public Profession Profession { get; set; }
}