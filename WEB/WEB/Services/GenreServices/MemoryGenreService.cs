using WEB.Domain.Entities;
using WEB.Domain.Models;

namespace WEB.Services.GenreServices
{
    public class MemoryGenreService
    {
        public Task<ResponseData<List<Genre>>> GetCategoryListAsync()
        {
            var categories = new List<Genre>
            {
                new Genre{ Id = 1, Name = "aaaa", NormalizedName = "AAAA"}
            };
        }
           

        
       
            
        
    }
}
