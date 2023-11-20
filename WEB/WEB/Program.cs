using WEB.Services.GenreServices;
using WEB.Services.MovieServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WEB.UriData;
using WEB.Services.ApiServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext") ?? throw new InvalidOperationException("Connection string 'MovieContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;

builder.Services.AddHttpClient<IMovieService, ApiMovieService>(opt => opt.BaseAddress = new Uri(UriData.ApiUri));

builder.Services.AddHttpClient<IGenreService, ApiGenreService>(opt => opt.BaseAddress = new Uri(UriData.ApiUri));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
