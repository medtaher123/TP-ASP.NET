using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TP3.Models;

namespace TP3.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Tp3Context _context;

        public CustomersController(Tp3Context context)
        {
            _context = context;
        }

        // Action to show the list of customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Include(c => c.Membershiptype) // Include Membershiptype data for display
                .Include(m => m.Movies) // Include Movies data for display
                .ToListAsync();

            return View(customers);
        }

        // GET: Create
        public IActionResult Create()
        {
            var members = _context.Membershiptypes.ToList();
            ViewBag.member = members.Select(members => new SelectListItem()
            {
                Text = members.Name,
                Value = members.Id.ToString()
            });

            // Get all movies to show in the movie selection
            var movies = _context.Movies.ToList();
            ViewBag.movies = new SelectList(movies, "Id", "Name");

            return View();
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(Customer customer, Guid[] selectedMovies)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate dropdown lists in case of errors
                var members = _context.Membershiptypes.ToList();
                ViewBag.member = members.Select(members => new SelectListItem()
                {
                    Text = members.Name,
                    Value = members.Id.ToString()
                });

                var movies = _context.Movies.ToList();
                ViewBag.movies = new SelectList(movies, "Id", "Name");

                // Store validation errors in ViewBag
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(customer);
            }

            // Assign selected movies to the customer
            if (selectedMovies != null)
            {
                customer.Movies = _context.Movies.Where(m => selectedMovies.Contains(m.Id)).ToList();
            }

            // Add the new customer to the database
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.Membershiptype) // Include Membershiptype details
                .Include(c => c.Movies)        // Include associated movies
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound(); // Handle case where the customer doesn't exist
            }

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.Movies) // Include associated movies
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Get all movies
            var allMovies = _context.Movies.ToList();

            // Create a list of movie checkboxes
            ViewBag.MovieList = allMovies.Select(movie => new SelectListItem
            {
                Value = movie.Id.ToString(),
                Text = movie.Name,
                Selected = customer.Movies.Any(cm => cm.Id == movie.Id)
            }).ToList();

            // Populate MembershipType dropdown
            ViewBag.MembershipTypes = _context.Membershiptypes
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                });

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Customer customer, Guid[] selectedMovies)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
        
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns if validation fails
                ViewBag.MembershipTypes = _context.Membershiptypes
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name
                    });
        
                ViewBag.MovieList = _context.Movies.Select(movie => new SelectListItem
                {
                    Value = movie.Id.ToString(),
                    Text = movie.Name,
                    Selected = selectedMovies.Contains(movie.Id)
                }).ToList();
        
                return View(customer);
            }
        
            try
            {
                // Fetch the existing customer and update movies
                var existingCustomer = await _context.Customers
                    .Include(c => c.Movies)
                    .FirstOrDefaultAsync(c => c.Id == id);
        
                if (existingCustomer == null)
                {
                    return NotFound();
                }
        
                // Update basic details
                existingCustomer.Name = customer.Name;
                existingCustomer.MembershiptypeId = customer.MembershiptypeId;
        
                // Update associated movies
                existingCustomer.Movies.Clear(); // Remove existing associations
                var selectedMovieEntities = _context.Movies.Where(m => selectedMovies.Contains(m.Id)).ToList();
                foreach (var movie in selectedMovieEntities)
                {
                    existingCustomer.Movies.Add(movie);
                }
        
                _context.Update(existingCustomer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.Id == customer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        
            return RedirectToAction(nameof(Index));
        }
        

    }
}   