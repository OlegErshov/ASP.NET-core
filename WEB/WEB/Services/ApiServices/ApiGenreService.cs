using System.Text;
using System.Text.Json;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.GenreServices;

namespace WEB.Services.ApiServices
{
    public class ApiGenreService : IGenreService
    {
        HttpClient _httpClient;
        int _pageSize;
        JsonSerializerOptions _serializerOptions;
        ILogger _logger;

        public ApiGenreService(HttpClient httpClient,
                                IConfiguration configuration,
                                ILogger<ApiMovieService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetValue<int>("ItemsPerPage");

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<List<Genre>>> GetCategoryListAsync()
        {
            // подготовка URL запроса
            var uri = _httpClient.BaseAddress?.AbsoluteUri + "genres/";
            var response = await _httpClient.GetAsync(uri);

           
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return  await response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<Genre>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
            return new ResponseData<List<Genre>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode.ToString()}"
            };
        }
    }
}
