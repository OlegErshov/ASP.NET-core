using Microsoft.AspNetCore.Mvc;
using WEB.Domain.Entities;
using WEB.Services.GenreServices;
using WEB.Services.MovieServices;

namespace WEB.Controllers
{
    public class MovieController : Controller
    {
        IMovieService _movieService;
        IGenreService _genreService;

        
        public Genre CurrentGenre { get; set; }

        List<Genre> Genres { get; set; }

        public MovieController(IMovieService movieService, IGenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index(string? genre)
        {
            Genres = _genreService.GetCategoryListAsync().Result.Data;
            ViewBag.Genres = Genres;

            CurrentGenre = _genreService.GetCategoryListAsync().Result.Data?.
                Find(x => x.NormalizedName.Equals(genre));
            if(CurrentGenre == null)
            {
                CurrentGenre = _genreService.GetCategoryListAsync().Result.Data.
                    Find(x => x.NormalizedName.Equals("action"));
            }

            ViewBag.CurrentGenre = CurrentGenre;

            var productResponse =
            await _movieService.GetProductListAsync(genre);
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data.Items);
        }
    }
}
