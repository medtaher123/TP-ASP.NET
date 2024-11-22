using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TP3.Models;

namespace TP3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Tp3Context _context;

        public MoviesController(Tp3Context context)
        {
            _context = context;
        }

        // Action to display all movies
        public async Task<IActionResult> Index()
        {
            // Fetch all movies from the database, along with their genres
            var movies = await _context.Movies.Include(m => m.Genre).ToListAsync();
            return View(movies); // Return the movies to the view
        }

        // Action to show the form for creating a new movie
        public IActionResult Create()
        {
            // Fetch all genres to populate the genre dropdown list in the form
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }

        // Action to handle the creation of a movie (POST)
       [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Check if a file is uploaded
                if (Image != null && Image.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
            
                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Store the image path in the Movie model
                    movie.ImagePath = "/images/" + uniqueFileName;

                    
                }
                // Add the movie to the database
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                    
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
