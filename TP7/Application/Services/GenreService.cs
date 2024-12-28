using TP7.Domain.Entities;
using TP7.Infrastructure.Persistence.Repositories;
using TP7.Infrastructure.Persistence.DBContexts;

namespace TP7.Application.Services
{
    public class GenreService 
    {
        private readonly GenericRepository<Genre> _genreRepository;

        public GenreService(GenericRepository<Genre> genreRepository)
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