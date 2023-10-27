namespace DataLayer.DbSets;

public class Genre
{
    public int Id { get; set; }
    public string GenreName { get; set; }
    public List<Title> Title { get; set; }
}