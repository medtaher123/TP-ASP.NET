using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP2.Models;

namespace TP2.Controllers;

public class GenreController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SqliteContext _db;


    public GenreController(SqliteContext db, ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var genres = _db.genres.ToList();
        return View(genres);
    }
    public IActionResult create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult create(Genre genre)
    {
        _db.genres.Add(genre);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        var genre = _db.genres.Find(id);
        return View(genre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult edit(Genre genre)
    {
        _db.genres.Update(genre);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult delete(Guid id)
    {
        var genre = _db.genres.Find(id);
        _db.genres.Remove(genre);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


}
