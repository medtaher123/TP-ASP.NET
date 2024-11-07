using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP2.Models;

namespace TP2.Controllers;

public class CustomerController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SqliteContext _db;


    public CustomerController(SqliteContext db, ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var customers = _db.customers.ToList();
        return Content(string.Join("\n", customers.Select(c => c.toString())));

    }

    public IActionResult AddCustomerPage()
    {
        return View();
    }


}
