using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder
    .Configuration
    .GetSection("isUri").Value;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidTypes =
    new[] { "at+jwt" };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7004")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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
app.UseCors("BlazorWasmPolicy");
app.UseAuthentication();
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