namespace WebServer.DataTransferObjects;

public class TitleDto
{
    public string Url { get; set; }
    public string Title { get; set; }
    public string TitleType { get; set; }
    public string? Poster { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public bool IsAdult { get; set; }
    public int? RunTimeMinutes { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
    public string? Plot { get; set; }
    public PersonalRatingDto? PersonalRatingDtos { get; set; }
    public List<GenreDto> Genres { get; set; }
    public List<PersonDto> PersonDtos { get; set; }
}