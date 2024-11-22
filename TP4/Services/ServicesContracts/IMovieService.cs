using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(Guid id);
        Task CreateMovieAsync(Movie movie, IFormFile image);
        Task UpdateMovieAsync(Movie movie, IFormFile image);
        Task DeleteMovieAsync(Guid id);
    }
}
