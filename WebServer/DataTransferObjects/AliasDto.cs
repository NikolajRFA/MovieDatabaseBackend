namespace WebServer.DataTransferObjects;

public class AliasDto
{
    public string TitleUrl { get; set; }
    public int Ordering { get; set; }
    public string Title { get; set; }
    public string Region { get; set; }
    public string Language { get; set; }
    public bool IsOriginalTitle { get; set; }
}