using System;
using System.Collections.Generic;

namespace TP3.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? MembershiptypeId { get; set; }

    public virtual Membershiptype? Membershiptype { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
