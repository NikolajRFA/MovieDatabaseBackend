namespace WebServer.DataTransferObjects;

public class UserDto
{
    public string Url { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Searches { get; set; }
    public string Bookmarks { get; set; }
    public string Ratings { get; set; }
}