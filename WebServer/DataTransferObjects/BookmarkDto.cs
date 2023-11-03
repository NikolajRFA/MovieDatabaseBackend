namespace WebServer.DataTransferObjects;

public class BookmarkDto
{
    public string Url { get; set; }
    public string User { get; set; }
    public string? Title { get; set; }
    public string? Person { get; set; }
}