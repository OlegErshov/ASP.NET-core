using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB.API.Data;
using WEB.Controllers;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.ApiServices;
using WEB.Services.GenreServices;
using WEB.Services.MovieServices;

namespace WEB.Tests
{
    public class MovieControllerUnitTest
    {
        private List<Genre> GetSampleGenres()
        {
            return new List<Genre>() {
                new Genre() { Id = 1, Name="Боевик", NormalizedName="action"},
                new Genre() { Id = 2, Name="Триллер", NormalizedName="thriller"}
            };
        }

        private List<Movie> GetSampleMovies()
        {
            return new List<Movie>()
                {
                    new Movie() { Id = 1, TicketPrice= 5, Title="Avengers", GenreId=2},
                    new Movie() { Id = 2, TicketPrice=10, Title="Iron man", GenreId=1},
                };
        }

        class GenreComparer : IEqualityComparer<Genre>
        {
            public bool Equals(Genre? x, Genre? y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                    return false;

                return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
            }

            public int GetHashCode(Genre obj)
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                hash = hash * 23 + obj.NormalizedName.GetHashCode();
                return hash;
            }
        }

        [Fact]
        public void MoviesListDoesntFound()
        {
            Mock<IGenreService> categories_moq = new();
            categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Genre>>()
            {
                Success = true,
                Data = GetSampleGenres()
            });

            Mock<IMovieService> movies_moq = new();
            movies_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Movie>>()
            {
                Success = false
            });

            var header = new Dictionary<string, StringValues>();
            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            controllerContext.HttpContext = moqHttpContext.Object;

            //Act
            var controller = new MovieController(movies_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
            var result = controller.Index(null).Result;
            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
        }

        [Fact]
        public void GenresListDoesntFound()
        {
            Mock<IGenreService> categories_moq = new();
            categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Genre>>()
            {
                Success = false
            });

            Mock<IMovieService> movies_moq = new();
            movies_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Movie>>()
            {
                Success = true
            });

            var header = new Dictionary<string, StringValues>();
            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            controllerContext.HttpContext = moqHttpContext.Object;

            //Act
            var controller = new MovieController(movies_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
            var result = controller.Index(null).Result;
            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
        }

        [Fact]
        public void GenresListUnitTest()
        {
            Mock<IGenreService> categories_moq = new();
            categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Genre>>()
            {
                Success = true,
                Data = GetSampleGenres()
            }) ;

            Mock<IMovieService> movies_moq = new();
            movies_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Movie>>()
            {
                Success = true,
                ErrorMessage = null,
                Data = new ListModel<Movie>()
                {
                    Items = GetSampleMovies()
                }
            });

            var header = new Dictionary<string, StringValues>();
            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            controllerContext.HttpContext = moqHttpContext.Object;

            var controller = new MovieController(movies_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
            var result = controller.Index(null).Result;

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("genres"));

            Assert.Equal(GetSampleGenres(), viewResult.ViewData["genres"] as IEnumerable<Genre>, new GenreComparer());
        }

        [Fact]
        public void CurrentGenreTest()
        {
            Mock<IGenreService> categories_moq = new();
            categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Genre>>()
            {
                Success = true,
                Data = GetSampleGenres()
            });

            Mock<IMovieService> movies_moq = new();
            movies_moq.Setup(m => m.GetProductListAsync(GetSampleGenres()[0].NormalizedName, 1)).ReturnsAsync(new ResponseData<ListModel<Movie>>()
            {
                Success = true,
                ErrorMessage = null,
                Data = new ListModel<Movie>()
                {
                    Items = GetSampleMovies()
                }
            });

            var header = new Dictionary<string, StringValues>();
            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            controllerContext.HttpContext = moqHttpContext.Object;

            var controller = new MovieController(movies_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
            var result = controller.Index(GetSampleGenres()[0].NormalizedName, 1).Result;

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("currentGenre"));

            Assert.Equal(GetSampleGenres()[0], viewResult.ViewData["currentGenre"] as Genre, new GenreComparer());
        }

        [Fact]
        public void CurrentGenreIsNullTest()
        {
            Mock<IGenreService> categories_moq = new();
            categories_moq.Setup(m => m.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Genre>>()
            {
                Success = true,
                Data = GetSampleGenres()
            });

            Mock<IMovieService> movies_moq = new();
            movies_moq.Setup(m => m.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Movie>>()
            {
                Success = true,
                ErrorMessage = null,
                Data = new ListModel<Movie>()
                {
                    Items = GetSampleMovies()
                }
            });

            var header = new Dictionary<string, StringValues>();
            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            controllerContext.HttpContext = moqHttpContext.Object;

            var controller = new MovieController(movies_moq.Object, categories_moq.Object) { ControllerContext = controllerContext };
            var result = controller.Index(null, 1).Result;

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("currentGenre"));

            Assert.Equal(GetSampleGenres()[0], viewResult.ViewData["currentGenre"] as Genre, new GenreComparer());
        }
    }

   
}
