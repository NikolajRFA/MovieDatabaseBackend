using WebServer.DataTransferObjects;

namespace DataLayer.DataTransferObjects;

public class MovieSearchResultDto : MovieSearchDropdownDto
{
    public int EndYear { get; set; }
    public string TitleType { get; set; }
}