﻿namespace DataLayer.DataTransferObjects;

public class TitleDto
{
    public string Title { get; set; }
    public string? Poster { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Type { get; set; }
    public bool IsAdult { get; set; }
    public int RunTimeMinutes { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
    public string? Plot { get; set; }
}