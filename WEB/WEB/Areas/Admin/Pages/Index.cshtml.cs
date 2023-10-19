using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB.Domain.Entities;
using WEB.Services.MovieServices;

namespace WEB.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _context;

        public IndexModel(IMovieService context)
        {
            _context = context;
        }

        public List<Movie> Movie { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.GetProductListAsync(null) != null)
            {
                Movie =  _context.GetProductListAsync(null).Result.Data.Items;
            }
        }
    }
}