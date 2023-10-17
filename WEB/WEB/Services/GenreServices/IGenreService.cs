using WEB.Domain.Models;
using WEB.Domain.Entities;

namespace WEB.Services.GenreServices
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
