using WEB.Domain.Entities;
using WEB.Domain.Models;

namespace WEB.Services.GenreServices
{
    public class MemoryGenreService : IGenreService
    {
        public Task<ResponseData<List<Genre>>> GetCategoryListAsync()
        {
            var categories = new List<Genre>
            {
                new Genre{ Id = 1, Name = "Action", NormalizedName = "action"},

                new Genre{Id = 2, Name = "Drama", NormalizedName = "drama"}
            };

            var result = new ResponseData<List<Genre>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
           

        
       
            
        
    }
}
