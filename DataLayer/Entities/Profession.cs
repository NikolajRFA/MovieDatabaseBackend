using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class Profession
{
    public int Id { get; set; }

    public string? Profession1 { get; set; }

    public virtual ICollection<Person> Nconsts { get; set; } = new List<Person>();
}
