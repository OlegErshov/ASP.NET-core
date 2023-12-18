using WEB.Domain.Entities;

namespace WEB.Blazor.Services
{
    public interface IDataService
    {
        // Список категорий объектов
        List<Genre> Genres { get; set; }
        //Список объектов
        List<Movie> Movies { get; set; }
        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя
        ///категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task GetMovieListAsync(
        string? categoryNormalizedName,
        int pageNo = 1);
        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не
        ///найден</returns>
        public Task<Movie?> GetMovieByIdAsync(int id);
        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <returns></returns>
        public Task GetGenreListAsync();

        public event Action? OnMovieChange;
    }
}
