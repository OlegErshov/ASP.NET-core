using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB.API.Services.MovieServices;
using WEB.Domain.Entities;
using WEB.Domain.Models;

namespace WEB.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _context;

        public MoviesController(IMovieService context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        
        [Route("page{pageNo:int}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string? genre,
                                                                      int pageNo = 1,
                                                                      int pageSize = 3)
        {
            return Ok(await _context.GetMovieListAsync(genre,pageNo,pageSize));
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
      
            var movie = await _context.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/Dishes/5
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _context.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMovie(int id, Movie movie)
        //{
        //    if (id != movie.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(movie).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (movie is null)
            {
                return BadRequest(new ResponseData<Movie>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "movie is null"
                });
            }
            var response = await _context.CreateMovieAsync(movie);

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return CreatedAtAction("GetMovie", new { id = movie.Id }, new ResponseData<Movie>()
            {
                Data = movie,
                Success = true
            });
        }

        // DELETE: api/Movies/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            //if (_context.Movies == null)
            //{
            //    return NotFound();
            //}
            var movie = await _context.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            // Зачем тут проверять на наличие, если это можно сделать в сервисе

            await _context.DeleteMovieAsync(id);
            // Не забыть добавить сохранение в бд в методах сервиса

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.GetMovieByIdAsync(id) == null;
        }
    }
}
