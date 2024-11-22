using System;
using System.Collections.Generic;

namespace TP4.Models;

public partial class Membershiptype
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal? SignUpFee { get; set; }

    public int? DurationInMonth { get; set; }

    public decimal? DiscountRate { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
