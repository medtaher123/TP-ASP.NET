using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP2.Models;

namespace TP2.Controllers;

public class MovieController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SqliteContext _db;


    public MovieController(SqliteContext db, ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var movies = _db.movies.ToList();
        return View(movies);
    }

    public IActionResult create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult create(Movie movie)
    {
        _db.movies.Add(movie);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var movie = _db.movies.Find(id);
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult edit(Movie movie)
    {
        _db.movies.Update(movie);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult delete(int id)
    {
        var movie = _db.movies.Find(id);
        _db.movies.Remove(movie);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }



}
