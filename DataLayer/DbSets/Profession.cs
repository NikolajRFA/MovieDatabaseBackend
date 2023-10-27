namespace DataLayer.DbSets;

public class Profession
{
    public int Id { get; set; }
    public string ProfessionName { get; set; }
    public List<Person> Person { get; set; }
}