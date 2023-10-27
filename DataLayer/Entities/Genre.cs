using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Genre
{
    public int Id { get; set; }

    public string? Genre1 { get; set; }

    public virtual ICollection<Title> Tconsts { get; set; } = new List<Title>();
}
