namespace WebServer.DataTransferObjects;

public class SearchDto
{
    public string Url { get; set; }
    public string User { get; set; }
    public string SearchPhrase { get; set; }
    public string DeleteUrl { get; set; }
    public DateTime Date { get; set; }
}