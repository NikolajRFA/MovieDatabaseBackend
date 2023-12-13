namespace DataLayer.DbSets;

public class Bookmark
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Tconst { get; set; }
    public Title Title { get; set; }
    public string? Nconst { get; set; }
    public Person Person { get; set; }
}