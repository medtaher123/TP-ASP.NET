using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using TP7.Domain.Entities;

using TP7.Application.Services;

namespace TP7.Presentation.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieService _movieService;
        private readonly GenreService _genreService;

        public MoviesController(MovieService movieService, GenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }

        // Action to display all movies
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return View(movies);
        }

        // Action to show the form for creating a new movie
        public async Task<IActionResult> Create()
        {
            var genres = await _genreService.GetAllGenresAsync();
                
            // Pass the genres to the view using ViewBag
            ViewBag.Genres = new SelectList(genres, "Id", "GenreName");
            return View();
        }

        // Action to handle the creation of a movie (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                await _movieService.CreateMovieAsync(movie, Image);
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        // Action to edit an existing movie
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Movie movie, IFormFile Image)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _movieService.UpdateMovieAsync(movie, Image);
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        // Action to delete a movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
