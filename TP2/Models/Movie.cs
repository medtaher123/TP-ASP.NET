namespace TP2.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Movie()
        {
        }
        public string toString()
        {
            return $"{Id} - {Name}";
        }
    }
}