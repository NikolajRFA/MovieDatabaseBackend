using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Person
{
    public string Nconst { get; set; } = null!;

    public string? Personname { get; set; }

    public string? Birthyear { get; set; }

    public string? Deathyear { get; set; }

    public decimal? NameRating { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Crew> Crews { get; set; } = new List<Crew>();

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
