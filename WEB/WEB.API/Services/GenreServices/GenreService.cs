using Microsoft.EntityFrameworkCore;
using WEB.API.Data;
using WEB.Domain.Entities;
using WEB.Domain.Models;

namespace WEB.API.Services.GenreServices
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseData<List<Genre>>> GetCategoryListAsync()
        {
            var genres = _context.Genres.AsQueryable();

            if(genres is not null)
            {
               return  new ResponseData<List<Genre>> 
               { 
                   Data = await genres.ToListAsync(),
                   Success = true,
                   ErrorMessage = string.Empty
               };
            }
            else
            {
                return new ResponseData<List<Genre>>
                {
                    Success = false,
                    ErrorMessage = "can't get genres from database",
                    Data = null
                };

            }
        }
    }
}
