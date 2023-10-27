using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Bookmark
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Tconst { get; set; }

    public string? Nconst { get; set; }

    public virtual Person? NconstNavigation { get; set; }

    public virtual Title? TconstNavigation { get; set; }

    public virtual User? User { get; set; }
}
