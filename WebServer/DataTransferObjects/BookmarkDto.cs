namespace WebServer.DataTransferObjects;

public class BookmarkDto
{
    public string Url { get; set; }
    public int UserId { get; set; }
    public string? Tconst { get; set; }
    public string? Nconst { get; set; }
}