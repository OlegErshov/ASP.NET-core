using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEB.Domain.Entities;

    public class MovieContext : DbContext
    {
        public MovieContext (DbContextOptions<MovieContext> options)
            : base(options)
        {

        }

        public DbSet<WEB.Domain.Entities.Movie> Movie { get; set; } = default!;
    }
