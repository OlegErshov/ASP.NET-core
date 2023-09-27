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
            // Выполнение миграций
            await context.Database.MigrateAsync();

            context.Movies.Add(new Movie
            {
                Title = "Iron Man",
                Description = "len pisat",
                TicketPrice = 2,
                ImgSrc = "images/iron-man.jpg",
                //Genre = _genres.Find(c => c.NormalizedName.Equals("action"))
            });

            context.Movies.Add(new Movie
            {
                Id = 2,
                Title = "Iron Man 2",
                Description = "len pisat",
                TicketPrice = 3,
                ImgSrc = "images/iron-man2.jpg",
                //Genre = _genres.Find(c => c.NormalizedName.Equals("action"))
            });
        }
    }

    
}
