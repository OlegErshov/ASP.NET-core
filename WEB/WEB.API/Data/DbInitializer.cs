using Microsoft.EntityFrameworkCore;
using WEB.Domain.Entities;

namespace WEB.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Получение урл адреса 
            string? url = app.Configuration.GetValue<string>("AppUrl");
            await context.Database.EnsureDeletedAsync();
            // Выполнение миграций
            await context.Database.MigrateAsync();

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
            await context.SaveChangesAsync();

            context.Add(new Movie
            {
                Title = "Iron Man",
                Description = "len pisat",
                TicketPrice = 2,
                ImgSrc = url + "/images/iron-man.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("action")).Id
            }) ;

            context.Add(new Movie
            {
                Title = "Iron Man 2",
                Description = "len pisat",
                TicketPrice = 3,
                ImgSrc = url + "/images/iron-man2.jpg",
                GenreId = context.Genres.FirstOrDefault(c => c.NormalizedName.Equals("action")).Id
            }) ;

            await context.SaveChangesAsync();
        }
    }

    
}
