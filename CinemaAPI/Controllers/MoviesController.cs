using CinemaAPI.Data;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public MoviesController(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        [Authorize(Roles = AppRoles.UserRole)]
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
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Movie movie)
        //{
        //    try
        //    {
        //        _cinemaDbContext.Movies.Add(movie);
        //        await _cinemaDbContext.SaveChangesAsync();

        //        return Created(HttpContext.Request.Scheme, movie);
        //    }
        //    catch (System.Exception x)
        //    {
        //        return BadRequest(x.InnerException?.Message ?? x.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Movie movie)
        {
            try
            {
                if (movie.Image != null)
                {
                    using (var fileStream = new FileStream(movie.GetDefaultPath(), FileMode.Create))
                    {
                        await movie.Image.CopyToAsync(fileStream);

                        movie.ImageURL = movie.GetDefaultPath();
                    }
                }

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
        public async Task<IActionResult> Put(long id, [FromForm] Movie movie)
        {
            try
            {
                var currentMovie = await _cinemaDbContext.Movies.FindAsync(id);

                currentMovie.Name = movie.Name;
                currentMovie.Language = movie.Language;
                currentMovie.Rate = movie.Rate;

                if (movie.Image != null)
                {
                    using (var fileStream = new FileStream(movie.GetDefaultPath(), FileMode.Create))
                    {
                        await movie.Image.CopyToAsync(fileStream);

                        currentMovie.ImageURL = movie.GetDefaultPath();
                    }
                }

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
