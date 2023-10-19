using Microsoft.AspNetCore.Mvc;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.GenreServices;

namespace WEB.Services.MovieServices
{
    public class MemoryMovieService : IMovieService
    {
        List<Movie> _movies;
        List<Genre> _genres;

        IConfiguration _config;
        public MemoryMovieService([FromServices] IConfiguration config,
                                    IGenreService genreService) {

            _genres = genreService.GetCategoryListAsync().Result.Data;

            _config = config;

            SetupData();
        }
        private void SetupData() 
        {
            _movies = new List<Movie>() {
                new Movie{Id = 1, Title = "Iron Man", Description = "len pisat",
                TicketPrice = 2, ImgSrc = "images/iron-man.jpg",
                Genre = _genres.Find(c => c.NormalizedName.Equals("action"))},

                new Movie{Id = 2,Title = "Iron Man 2", Description = "len pisat",
                TicketPrice = 3, ImgSrc = "images/iron-man2.jpg",
                Genre = _genres.Find(c => c.NormalizedName.Equals("action"))}
            };
        }
        public Task<ResponseData<Movie>> CreateProductAsync(Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movie>> GetProductByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        
        public  Task<ResponseData<ListModel<Movie>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {

            var itemsOnPage = _config.GetValue<int>("ItemsPerPage");


            int totalPages;
            List<Movie>  result = new List<Movie>();

            if (categoryNormalizedName == null)
            {
                totalPages = _movies.Count();
                result = _movies.Skip((pageNo - 1) * itemsOnPage).Take(itemsOnPage).ToList();

            }
            else
            {
                 totalPages = _movies!.Where(x => x.Genre.NormalizedName == null ||
                                    x.Genre.NormalizedName.Equals(categoryNormalizedName)).
                                    ToList().Count() / itemsOnPage + 1;

                result = _movies!.Where(x => x.Genre.NormalizedName == null ||
                                        x.Genre.NormalizedName.Equals(categoryNormalizedName)).ToList().
                                        Skip((pageNo - 1) * itemsOnPage).Take(itemsOnPage).ToList();
            }

             
            
            return Task.FromResult(new ResponseData<ListModel<Movie>>
            {
                Data = new ListModel<Movie>
                {
                    Items = result.ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                },
                Success = true,
                ErrorMessage = string.Empty

            }) ; 
        }

        public Task UpdateProductAsync(int id, Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync() 
        { 
            throw new NotImplementedException();
        }
    }
}
