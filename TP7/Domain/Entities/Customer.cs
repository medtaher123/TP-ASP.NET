using System;
using System.Collections.Generic;

namespace TP7.Domain.Entities;

public partial class Customer
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? MembershiptypeId { get; set; }

    public virtual Membershiptype? Membershiptype { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
