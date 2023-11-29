namespace WebServer.DataTransferObjects;

public class CrewDto
{
    public string Url { get; set; }
    public string Title { get; set; }
    public int Ordering { get; set; }
    public string Person { get; set; }
    public string PersonName { get; set; }
    public string Category { get; set; }
    public string? Job { get; set; } 
    public string? Character { get; set; }
}