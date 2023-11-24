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
        IHttpContextAccessor _httpContextAccessor;
        IWebHostEnvironment _environment;
        public MovieService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }
       
      //  private var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        public async  Task<ResponseData<Movie>> CreateMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResponseData<Movie>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
            return new ResponseData<Movie>()
            {
                Data = movie,
                Success = true
            };
        }

        public Task DeleteMovieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? categoryNormalizedName, int pageNo = 1,int pageSize = 3)
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

        public Task UpdateMovieAsync(int id, Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            var imageFolder = Path.Combine(_environment.WebRootPath, "Images");
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!String.IsNullOrEmpty(movie.ImgSrc))
                {
                    var prevImage = Path.GetFileName(movie.ImgSrc);
                    
                }
                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                // Сохранить файл
                
                // Указать имя файла в объекте
                movie.ImgSrc = $"{host}/Images/{fName}";
                await _context.SaveChangesAsync();
            }
            responseData.Data = movie.ImgSrc;
            return responseData;
        }
    }
}
