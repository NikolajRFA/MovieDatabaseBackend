namespace WebServer.DataTransferObjects;

public class BookmarkDto
{
    public string Url { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Person { get; set; }
}