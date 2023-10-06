using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WEB.API.Data;
using WEB.Domain.Entities;
using WEB.Domain.Models;

//using var scope = app.Services.CreateScope();

namespace WEB.API.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly int _maxPageSize = 20;
        AppDbContext _context;
        public MovieService(AppDbContext context) 
        {
            _context = context;
        }
       
      //  private var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        public Task<ResponseData<Movie>> CreateProductAsync(Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movie>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Movie>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1,int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Movies.AsQueryable();
            var dataList = new ListModel<Movie>();
            query = query
            .Where(d => categoryNormalizedName == null
            || d.Genre.NormalizedName.Equals(categoryNormalizedName));

            // количество элементов в списке
            var count = await query.CountAsync();
            if (count == 0)
            {
                return new ResponseData<ListModel<Movie>>
                {
                    Data = dataList
                };
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Movie>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };
            dataList.Items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            var response = new ResponseData<ListModel<Movie>>
            {
                Data = dataList
            };
            return response;
        }

        public Task UpdateProductAsync(int id, Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}
