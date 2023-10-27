namespace DataLayer.DbSets;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Search> Searches { get; set; }
    public List<Rating> Ratings { get; set; }
}