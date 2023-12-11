namespace WebServer.DataTransferObjects;

public class PersonDto
{
    public string Url { get; set; }
    public string Name { get; set; }
    public string BirthYear { get; set; }
    public string DeathYear { get; set; }
    public double NameRating { get; set; }
    public List<ProfessionDto> Professions { get; set; }
    public string TitlesUrl { get; set; }
}