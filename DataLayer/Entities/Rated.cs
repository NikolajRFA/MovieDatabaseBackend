using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Rated
{
    public string Tconst { get; set; } = null!;

    public int Id { get; set; }

    public int Rating { get; set; }

    public DateTime? Date { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual Title TconstNavigation { get; set; } = null!;
}
