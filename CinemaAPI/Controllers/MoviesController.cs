using CinemaAPI.Data;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public MoviesController(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _cinemaDbContext.Movies.ToListAsync());
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

            if (currentMovie == null) return NotFound();

            return Ok(currentMovie);
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie movie)
        {
            try
            {
                _cinemaDbContext.Movies.Add(movie);
                await _cinemaDbContext.SaveChangesAsync();

                return Created(HttpContext.Request.Scheme, movie);
            }
            catch (System.Exception x)
            {
                return BadRequest(x.InnerException?.Message ?? x.Message);
            }
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Movie movie)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

                currentMovie.Name = movie.Name;
                currentMovie.Language = movie.Language;

                _cinemaDbContext.Movies.Update(currentMovie);
                await _cinemaDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception x)
            {
                return BadRequest(x.InnerException?.Message ?? x.Message);
            }
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

                _cinemaDbContext.Movies.Remove(currentMovie);
                await _cinemaDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception x)
            {
                return BadRequest(x.InnerException?.Message ?? x.Message);
            }
        }
    }
}
