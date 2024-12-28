using System;
using System.Collections.Generic;

namespace TP7.Domain.Entities;

public partial class Genre
{
    public Guid Id { get; set; }

    public string? GenreName { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
