namespace DataLayer.DataTransferObjects;

public class MovieSearchDropdownDto
{
    public string Title { get; set; }
    public int StartYear { get; set; }
    public List<PersonDto> PersonDtos { get; set; }
    public string Poster { get; set; }
}