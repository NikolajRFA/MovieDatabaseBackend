namespace DataLayer.DbSets;

public class IsEpisodeOf
{
    public string Tconst { get; set; }
    public string ParentTconst { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public Title Title { get; set; }
    public Title ParentTitle { get; set; }
}