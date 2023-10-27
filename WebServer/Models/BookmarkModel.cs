namespace WebServer.Models;

public class BookmarkModel
{
    public int UserId { get; set; }
    // TitlePersonId can either be a person or a title - to be bookmarked
    public string TitlePersonId { get; set; }
}