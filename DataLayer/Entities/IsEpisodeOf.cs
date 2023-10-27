using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class IsEpisodeOf
{
    public string Tconst { get; set; } = null!;

    public string Parenttconst { get; set; } = null!;

    public int? Seasonnumber { get; set; }

    public int? Episodenumber { get; set; }

    public virtual Title ParenttconstNavigation { get; set; } = null!;

    public virtual Title TconstNavigation { get; set; } = null!;
}
