using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TP4.Models;

namespace TP4.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Action to show the list of customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            var members = await _customerService.GetAllMembershipTypesAsync();
            ViewBag.member = members.Select(members => new SelectListItem
            {
                Text = members.Name,
                Value = members.Id.ToString()
            });

            var movies = await _customerService.GetAllMoviesAsync();
            ViewBag.movies = new SelectList(movies, "Id", "Name");

            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer, Guid[] selectedMovies)
        {
            if (!ModelState.IsValid)
            {
                var members = await _customerService.GetAllMembershipTypesAsync();
                ViewBag.member = members.Select(members => new SelectListItem
                {
                    Text = members.Name,
                    Value = members.Id.ToString()
                });

                var movies = await _customerService.GetAllMoviesAsync();
                ViewBag.movies = new SelectList(movies, "Id", "Name");

                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(customer);
            }

            await _customerService.CreateCustomerAsync(customer, selectedMovies);
            return RedirectToAction(nameof(Index));
        }

        // Action to display the customer details
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var movies = await _customerService.GetAllMoviesAsync();
            ViewBag.MovieList = movies.Select(movie => new SelectListItem
            {
                Value = movie.Id.ToString(),
                Text = movie.Name,
                Selected = customer.Movies.Any(cm => cm.Id == movie.Id)
            }).ToList();

            var membershipTypes = await _customerService.GetAllMembershipTypesAsync();
            ViewBag.MembershipTypes = membershipTypes
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                });

            return View(customer);
        }

        // POST: Edit
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
                var membershipTypes = await _customerService.GetAllMembershipTypesAsync();
                ViewBag.MembershipTypes = membershipTypes
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name
                    });

                ViewBag.MovieList = await _customerService.GetAllMoviesAsync();
                return View(customer);
            }

            try
            {
                await _customerService.EditCustomerAsync(customer, selectedMovies);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
