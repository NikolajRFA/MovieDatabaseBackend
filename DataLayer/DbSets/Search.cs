namespace DataLayer.DbSets;

public class Search
{
    public int Id { get; set; }
    public string SearchPhrase { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; }
}