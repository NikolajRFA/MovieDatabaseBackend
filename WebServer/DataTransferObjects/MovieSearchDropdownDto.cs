﻿using DataLayer.DataTransferObjects;

namespace WebServer.DataTransferObjects;

public class MovieSearchDropdownDto
{
    public string Url { get; set; }
    public string Title { get; set; }
    public int StartYear { get; set; }
    public List<PersonNameDto> PersonDtos { get; set; }
    public string Poster { get; set; }
}