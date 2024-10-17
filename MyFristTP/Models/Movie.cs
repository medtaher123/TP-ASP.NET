using System.ComponentModel;

namespace MyFristTP.Models
{
    public class Movie
    {
        public int ID { get; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        private Movie(int id, string title, DateTime releaseDate, string genre, decimal price)
        {
            ID = id;
            Title = title;
            ReleaseDate = releaseDate;
            Genre = genre;
            Price = price;
        }

        public static Movie create(string title, DateTime releaseDate, string genre, decimal price)
        {
            int id = new Random().Next(1000);
            return new Movie(id, title, releaseDate, genre, price);
        }
        public override string ToString()
        {
            return $"ID: {ID}, Title: {Title}, Release Date: {ReleaseDate}, Genre: {Genre}, Price: {Price}";
        }
    }

    public class MovieList
    {

        public List<Movie> Movies { get; private set; }


        public MovieList()
        {
            init();
        }
        private void init()
        {
            if (Movies == null)
            {
                Movies = new List<Movie>{
                    Movie.create("The Shawshank Redemption", new DateTime(1994, 10, 14), "Drama", 8.75m),
                    Movie.create("The Godfather", new DateTime(1972, 3, 24), "Crime", 8.65m),
                    Movie.create("The Dark Knight", new DateTime(2008, 7, 18), "Action", 9.0m),
                    Movie.create("Pulp Fiction", new DateTime(1994, 10, 14), "Crime", 8.5m),
                    Movie.create("The Lord of the Rings: The Return of the King", new DateTime(2003, 12, 17), "Adventure", 8.75m)
                };
            }
        }

        public Movie Find(int id)
        {
            return Movies.Find(m => m.ID == id);
        }

        public void Add(Movie movie)
        {
            if (Movies.Any(m => m.ID == movie.ID))
            {
                throw new Exception("The movie already exists");
            }
            Movies.Add(movie);
        }

        public override string ToString()
        {
            return string.Join("\n", Movies);
        }

        public List<Movie> All()
        {
            return Movies;
        }

        public List<Movie> Released(int year, int month)
        {
            return Movies.FindAll(m => m.ReleaseDate.Year == year && m.ReleaseDate.Month == month);
        }

    }

}

