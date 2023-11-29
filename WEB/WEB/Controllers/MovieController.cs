using Microsoft.AspNetCore.Mvc;
using WEB.Domain.Entities;
using WEB.Extensions;
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

        public async Task<IActionResult> Index(string? genre,int pageNo = 1)
        {
            Genres = _genreService.GetCategoryListAsync().Result.Data;

            CurrentGenre = _genreService.GetCategoryListAsync().Result.Data?.
                Find(x => x.NormalizedName.Equals(genre));

            if(CurrentGenre == null)
            {
                // Здесь налл
                CurrentGenre = _genreService?.GetCategoryListAsync()?.Result?.Data[0];
                    
            }
            var movieResponse = await _movieService.GetProductListAsync(CurrentGenre.NormalizedName, pageNo);

            ViewData["genres"] = Genres;
            ViewData["currentGenre"] = CurrentGenre;
            ViewData["currentPage"] = movieResponse.Data!.CurrentPage;
            ViewData["totalPages"] = movieResponse.Data.TotalPages;

            if (Request.isAjaxRequest())
            {
                return PartialView("Partials/_MovieCatalogPartial", new
                {
                    Movie = movieResponse.Data!.Items,
                    Genre = genre,
                    movieResponse.Data!.CurrentPage,
                    movieResponse.Data!.TotalPages,
                    ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
                    IsAdmin = false
                });
            }

            return View(movieResponse.Data!.Items);
        }
    }
}
