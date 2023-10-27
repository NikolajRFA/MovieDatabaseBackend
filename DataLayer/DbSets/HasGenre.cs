namespace DataLayer.DbSets;

public class HasGenre
{
    public int Id { get; set; }
    public string Tconst { get; set; }
    public Genre Genre { get; set; }
    public Title Title { get; set; }
}