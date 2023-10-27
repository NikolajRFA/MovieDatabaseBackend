using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Rated> Rateds { get; set; } = new List<Rated>();

    public virtual ICollection<Search> Searches { get; set; } = new List<Search>();
}
