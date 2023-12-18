using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using WEB.Domain.Models;
using WEB.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;

namespace WEB.Blazor.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public List<Genre>? Genres { get; set; }
        public List<Movie>? Movies { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public event Action? OnMovieChange;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetValue<int>("PageSize");
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task GetGenreListAsync()
        {
            //var tokenResult = await _accessTokenProvider.RequestAccessToken();
            //if (tokenResult.TryGetToken(out var token))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            //}
            var uri = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}genres/");
            string path = uri.ToString();
            var response = await _httpClient.GetAsync(new Uri(uri.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var genres = await response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>(_jsonSerializerOptions);
                    Genres = genres.Data;
                    Success = true;
                }
                catch (JsonException ex)
                {
                    ErrorMessage = $"Что-то пошло не так при запросе жанров. Ошибка: {ex.Message}";
                    Success = false;
                }
            }
            else
            {
                ErrorMessage = "Что-то пошло не так при запросе жанров.";
                Success = false;
            }
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            //var tokenResult = await _accessTokenProvider.RequestAccessToken();
            //if (tokenResult.TryGetToken(out var token))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            //}
            var uri = _httpClient.BaseAddress?.AbsoluteUri + $"movies/{id}";

            string path = uri.ToString();

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    Success = true;
                    return (await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_jsonSerializerOptions))?.Data;
                }
                catch (JsonException ex)
                {
                    ErrorMessage = $"Что-то пошло не так при запросе картинки. Ошибка: {ex.Message}";
                    Success = false;
                    return null;
                }
            }
            else
            {
                ErrorMessage = "Что-то пошло не так при запросе картинки.";
                Success = false;
                return null;
            }
        }

        public async Task GetMovieListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            //var tokenResult = await _accessTokenProvider.RequestAccessToken();
            //if (tokenResult.TryGetToken(out var token))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            //}
            var uri = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}movies/");

            if (genreNormalizedName != null)
                uri.Append($"{genreNormalizedName}/");

            if (pageNo >= 1)
                uri.Append($"page{pageNo}");

            if (!_pageSize.Equals("3"))
                uri.Append(QueryString.Create("pageSize", _pageSize.ToString()));

            string path = uri.ToString();
            var response = await _httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Movie>>>(_jsonSerializerOptions);
                    Movies = responseData?.Data.Items;
                    TotalPages = responseData?.Data?.TotalPages ?? 0;
                    CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                    Success = true;
                    OnMovieChange?.Invoke();
                }
                catch (JsonException ex)
                {
                    ErrorMessage = $"Что-то пошло не так при запросе кинофильмов. Ошибка: {ex.Message}";
                    Success = false;
                }
            }
            else
            {
                ErrorMessage = "Что-то пошло не так при запросе кинофильмов.";
                Success = false;
            }
        }


    }
}
