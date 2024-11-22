using System;
using System.Collections.Generic;

namespace TP3.Models;

public partial class Genre
{
    public Guid Id { get; set; }

    public string? GenreName { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
