using Microsoft.AspNetCore.Mvc;
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

        // Action pour afficher la liste des clients
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Include(c => c.Membershiptype) // Inclure les données de Membershiptype pour l'affichage
                .ToListAsync();

            return View(customers);
        }
    }
}
