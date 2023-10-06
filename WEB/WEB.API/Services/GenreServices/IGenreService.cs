using WEB.Domain.Entities;
using WEB.Domain.Models;

namespace WEB.API.Services.GenreServices
{
    public interface IGenreService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Genre>>> GetCategoryListAsync();
    }
}
