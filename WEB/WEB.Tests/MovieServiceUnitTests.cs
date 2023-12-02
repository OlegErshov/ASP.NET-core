using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB.API.Data;
using WEB.API.Services.MovieServices;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Services.ApiServices;

namespace WEB.Tests
{
    public class MovieServiceUnitTests
    {

        AppDbContext _сontext;
        public MovieServiceUnitTests()
        {
            var connection = new SqliteInMemoryConnection();
            _сontext = connection.CreateContext();
        }

        [Fact]
        public void ServiceReturnsFirstPageOfThreeItems()
        {
         
            var service = new MovieService(_сontext, null, null);
            var result = service.GetMovieListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Movie>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(_сontext.Movies.First(), result.Data.Items[0]);
        }

        [Fact]
        public void ServiceFilterGenres()
        {
            var service = new MovieService(_сontext, null, null);
            var result = service.GetMovieListAsync("adventure").Result;
            Assert.IsType<ResponseData<ListModel<Movie>>>(result);
            Assert.True(result.Success);
            Assert.DoesNotContain(result.Data.Items, x => x.GenreId != 2);
        }

        [Fact]
        public void ServiceMaxPageSizeAmountTest()
        {
            var service = new MovieService(_сontext, null!, null!);
            var result = service.GetMovieListAsync(null,1, service.MaxPageSize + 1).Result;

            Assert.True(result.Success);
            Assert.False(result.Data!.Items.Count <= service.MaxPageSize);
        }

        [Fact]
        public void ServiceMaxPageCountAmountTest()
        {
            var service = new MovieService(_сontext, null!, null!);
            var result = service.GetMovieListAsync(null, 100000).Result;

            Assert.False(result.Success);
        }
    }
}
