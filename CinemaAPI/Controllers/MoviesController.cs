using CinemaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private List<Movie> movies = new List<Movie>
        {
            new Movie{Id = 0 , Name = "AOT"},
            new Movie {Id = 1 ,Name = "War of Justice"}
        };

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return movies;
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            movies.Add(movie);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Movie movie)
        {
            movies[id] = movie;
        }

        [HttpDelete]
        public void Delete(long id)
        {
            var currentMovie = movies.Find(item => item.Id == id);

            movies.Remove(currentMovie);
        }
    }
}
