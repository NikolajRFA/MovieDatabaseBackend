using DataLayer.DataTransferObjects;

namespace WebServer.DataTransferObjects
{
    public class RatingDto
    {
        public TitleDto Title { get; set; }
        public UserDto User { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; } 
    }
}
