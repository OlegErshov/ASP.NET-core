using Microsoft.EntityFrameworkCore;
using System.Runtime;
using WEB.API.Data;
using WEB.API.Services.GenreServices;
using WEB.API.Services.MovieServices;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("SqLiteConnection");

builder.Services.AddDbContext<AppDbContext>(opt =>
                                opt.UseSqlite(connString));

// Add services to the container.
builder.Services.AddScoped<IMovieService,MovieService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();
await DbInitializer.SeedData(app);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();


app.MapControllerRoute(
    name: "Movies",
    pattern: "/api/movies/{pageNo:int}",
    defaults: new { controller = "MoviesController", action = "GetMovies" }
    );
app.MapControllerRoute(
    name: "Movies",
    pattern: "/api/movies/{genre:string}/{pageNo:int}",
    defaults: new { controller = "MoviesController", action = "GetMovies" }
    );
app.MapControllerRoute(
    name: "Movies",
    pattern: "/api/movies",
    defaults: new { controller = "MoviesController", action = "GetMovies" }
    );