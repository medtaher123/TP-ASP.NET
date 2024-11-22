using TP4.Models;

namespace TP4.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenericRepository<Genre> _genreRepository;

        public GenreService(IGenericRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // Method to get all genres
        public Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return _genreRepository.GetAllAsync();
        }
    }
}