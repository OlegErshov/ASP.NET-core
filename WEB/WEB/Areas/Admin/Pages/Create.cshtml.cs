using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB.Domain.Entities;
using WEB.Services.GenreServices;
using WEB.Services.MovieServices;

namespace WEB.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IMovieService _context;

        private readonly IGenreService _genreContext;

        public CreateModel(IMovieService context, IGenreService genreService)
        {
            _context = context;
            _genreContext = genreService;
        }

        public IActionResult OnGet()
        {
            ViewData["GenreId"] = new SelectList(_genreContext.GetCategoryListAsync().Result.Data, "Id", "Name");
                return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Надо как-то страницу получать и жанр кино, если надо

            if (!ModelState.IsValid || _context.GetProductListAsync(null) == null || Movie == null)
            {
                return Page();
            }

            await _context.CreateProductAsync(Movie, Image) ;

            return RedirectToPage("./Index");
        }
    }
}
