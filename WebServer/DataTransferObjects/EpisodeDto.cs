namespace WebServer.DataTransferObjects;

public class EpisodeDto
{
    public string ParentUrl { get; set; }
    public string EpisodeUrl { get; set; }
    public string Title { get; set; }
    public int Season { get; set; }
    public int Episode { get; set; }
}