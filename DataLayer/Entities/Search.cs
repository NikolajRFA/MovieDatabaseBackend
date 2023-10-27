using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Search
{
    public int Id { get; set; }

    public string Searchphrase { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}
