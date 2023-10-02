using Microsoft.EntityFrameworkCore;
using WEB.Domain.Entities;

namespace WEB.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) {
            
        }
       
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }
    }
}
