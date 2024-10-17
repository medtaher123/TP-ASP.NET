using Microsoft.AspNetCore.Mvc;
using MyFristTP.Models;
using System.Collections.Generic;

namespace MyFirstTP.Controllers
{
    [Route("[controller]")]

    public class MovieController : Controller
    {

        private static MovieList movies = new MovieList();


        [HttpGet]
        public IActionResult Index()
        {
            return View(movies.All());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            Movie movie = movies.Find(id);
            if (movie == null)
                return Content("Movie not found");
            return Content(movie.ToString());
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            movies.Add(movie);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Movie movie)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //movies.RemoveAll(m => m.ID == id);
        }

        [HttpGet("released/{year:int}/{month:int}")]
        public IActionResult Released(int year, int month)
        {
            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;

            return View("index", movies.Released(year, month));
        }
    }
}
// Compare this snippet from Startup.cs:
