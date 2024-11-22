using TP4.Models;

namespace TP4.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
    }
}