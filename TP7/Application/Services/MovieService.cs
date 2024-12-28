using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TP7.Domain.Entities;
using TP7.Infrastructure.Persistence.Repositories;


namespace TP7.Application.Services
{
    public class MovieService
    {
        private readonly GenericRepository<Movie> _movieRepository;

        public MovieService(GenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _movieRepository.GetAllAsync(movies => movies.Include(m => m.Genre));
        }

        public async Task<Movie> GetMovieByIdAsync(Guid id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task CreateMovieAsync(Movie movie, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                movie.ImagePath = "/images/" + uniqueFileName;
            }

            await _movieRepository.AddAsync(movie);
        }

        public async Task UpdateMovieAsync(Movie movie, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                movie.ImagePath = "/images/" + uniqueFileName;
            }

            await _movieRepository.UpdateAsync(movie);
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            await _movieRepository.DeleteAsync(id);
        }
    }
}
