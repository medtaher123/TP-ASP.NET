using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    // Display all movies
    public IActionResult Index()
    {
        var movieEntities = _db.movies.Include(m => m.Genre).ToList();
        var movieDtos = MovieMapper.ToDtoList(movieEntities);
        return View(movieDtos);
    }

    // Create movie (GET)
    public IActionResult Create()
    {
        ViewBag.Genres = new SelectList(_db.genres, "Id", "Name");
        return View();
    }

    // Create movie (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MovieDto movieDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Genres = new SelectList(_db.genres, "Id", "Name");
            return View(movieDto);
        }

        var movieEntity = MovieMapper.ToEntity(movieDto);
        _db.movies.Add(movieEntity);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Edit movie (GET)
    public IActionResult Edit(int id)
    {
        var movieEntity = _db.movies.Find(id);
        if (movieEntity == null)
        {
            return NotFound();
        }

        var movieDto = MovieMapper.ToDto(movieEntity);
        ViewBag.Genres = new SelectList(_db.genres, "Id", "Name", movieEntity.GenreId);
        return View(movieDto);
    }

    // Edit movie (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(MovieDto movieDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Genres = new SelectList(_db.genres, "Id", "Name", movieDto.GenreId);
            return View(movieDto);
        }

        var movieEntity = MovieMapper.ToEntity(movieDto);
        _db.movies.Update(movieEntity);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Delete movie
    public IActionResult Delete(int id)
    {
        var movieEntity = _db.movies.Find(id);
        if (movieEntity == null)
        {
            return NotFound();
        }

        _db.movies.Remove(movieEntity);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
