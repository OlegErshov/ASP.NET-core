using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.GenreServices;

namespace WEB.Services.ApiServices
{
    public class ApiGenreService : IGenreService
    {
        public Task<ResponseData<List<Genre>>> GetCategoryListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
