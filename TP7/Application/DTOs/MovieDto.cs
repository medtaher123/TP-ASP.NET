using System;

namespace TP7.Application.DTOs
{
    public class MovieDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public Guid? GenreId { get; set; }
    }
}
