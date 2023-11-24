using Azure.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.MovieServices;

namespace WEB.Services.ApiServices
{
    public class ApiMovieService : IMovieService
    {
        HttpClient _httpClient;
        int _pageSize;
        JsonSerializerOptions _serializerOptions;
        ILogger _logger;
        public ApiMovieService(HttpClient httpClient,
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
            
        public async  Task<ResponseData<Movie>> CreateProductAsync(Movie product, IFormFile? formFile)
        {
            
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "movies");

            var response = await _httpClient.PostAsJsonAsync(uri,product,_serializerOptions);
            if (response.IsSuccessStatusCode)
            {
               var data = await response
                         .Content
                         .ReadFromJsonAsync<ResponseData<Movie>>
                         (_serializerOptions);

                if (formFile is not null)
                {
                    var id = data.Data.Id;
                    await SaveImageAsync(id,formFile);
                }

                return data; // movie;
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
            return new ResponseData<Movie>
            { 
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{ response.StatusCode.ToString() }"
            };
           
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movie>> GetProductByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Movie>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}movies/");
            
            // добавить номер страницы в маршрут
            if (pageNo >= 1)
            {
                urlString.Append($"page{pageNo}?");
            };
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }
            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Movie>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Movie>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{ response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Movie>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error:{ response.StatusCode.ToString() }"
            };
        }

        public Task UpdateProductAsync(int id, Movie product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }


        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Movies/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }
    }
}
