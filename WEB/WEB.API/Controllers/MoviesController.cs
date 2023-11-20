using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.API.Data;
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
            return Ok(await _context.GetProductListAsync(genre,pageNo,pageSize));
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
      
            var movie = await _context.GetProductByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/Dishes/5
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
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            //if (_context.Movies == null)
            //{
            //    return Problem("Entity set 'AppDbContext.Movies'  is null.");
            //}
            await _context.CreateProductAsync(movie,null);
            

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            //if (_context.Movies == null)
            //{
            //    return NotFound();
            //}
            var movie = await _context.GetProductByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            // Зачем тут проверять на наличие, если это можно сделать в сервисе

            await _context.DeleteProductAsync(id);
            // Не забыть добавить сохранение в бд в методах сервиса

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.GetProductByIdAsync(id) == null;
        }
    }
}
