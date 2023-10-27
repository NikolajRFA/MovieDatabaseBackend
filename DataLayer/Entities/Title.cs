using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Title
{
    public string Tconst { get; set; } = null!;

    public string? Titletype { get; set; }

    public string? Primarytitle { get; set; }

    public string? Originaltitle { get; set; }

    public bool? Isadult { get; set; }

    public string? Startyear { get; set; }

    public string? Endyear { get; set; }

    public int? Runtimeminutes { get; set; }

    public string? Plot { get; set; }

    public decimal? Averagerating { get; set; }

    public int? Numvotes { get; set; }

    public string? Poster { get; set; }

    public virtual ICollection<Alias> Aliases { get; set; } = new List<Alias>();

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Crew> Crews { get; set; } = new List<Crew>();

    public virtual ICollection<IsEpisodeOf> IsEpisodeOfParenttconstNavigations { get; set; } = new List<IsEpisodeOf>();

    public virtual ICollection<IsEpisodeOf> IsEpisodeOfTconstNavigations { get; set; } = new List<IsEpisodeOf>();

    public virtual ICollection<Rated> Rateds { get; set; } = new List<Rated>();

    public virtual ICollection<Genre> Ids { get; set; } = new List<Genre>();
}
