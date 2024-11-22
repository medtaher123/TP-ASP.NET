using System;
using System.Collections.Generic;

namespace TP4.Models;

public partial class Movie
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime ReleaseDate { get; set; }  // Date de sortie du film
        
    public string? ImagePath { get; set; }  // Path to the image file

    public Guid? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
