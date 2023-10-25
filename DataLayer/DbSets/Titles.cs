namespace DataLayer.DbSets;

public class Title
{
    public string Tconst { get; set; }
    public string TitleType { get; set; }
    public string OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public int RuntimeMinutes { get; set; }
    public string Plot { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
    public string Poster { get; set; }
}