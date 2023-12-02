using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WEB.API.Data;
using WEB.Domain.Entities;

namespace WEB.Tests
{
    public class SqliteInMemoryConnection
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public SqliteInMemoryConnection()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new AppDbContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                //viewCommand.CommandText = @"
                //    CREATE VIEW AllResources AS
                //    SELECT Url
                //    FROM Blogs;";
                viewCommand.ExecuteNonQuery();
            }
            context.Add(new Genre
            {
                Name = "Action",
                NormalizedName = "action"

            });

            context.Add(new Genre
            {
                Name = "Adventure",
                NormalizedName = "adventure"

            });

            context.Add(new Genre
            {
                Name = "Drama",
                NormalizedName = "drama"

            });

            context.Add(new Genre
            {
                Name = "Thriller",
                NormalizedName = "thriller"

            });
            context.SaveChanges();

            context.Add(new Movie
            {
                Title = "Iron Man",
                Description = "len pisat",
                TicketPrice = 2,
                ImgSrc = "/images/iron-man.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("action")).Id
            });

            context.Add(new Movie
            {
                Title = "Iron Man 2",
                Description = "len pisat",
                TicketPrice = 3,
                ImgSrc = "/images/iron-man2.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("action")).Id
            });

            context.Add(new Movie
            {
                Title = "Iron Man 3",
                Description = "Last iron man movie",
                TicketPrice = 10,
                ImgSrc = "/images/iron-man2.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("drama")).Id
            });

            context.Add(new Movie
            {
                Title = "Avengers",
                Description = "AVENGERS assemble",
                TicketPrice = 100,
                ImgSrc = "/images/iron-man2.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("action")).Id
            });
            context.SaveChanges();
        }

        public AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
    }
}
