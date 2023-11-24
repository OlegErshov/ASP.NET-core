using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB.Domain.Entities;
using WEB.Services.GenreServices;
using WEB.Services.MovieServices;

namespace WEB.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IMovieService _context;

        private readonly IGenreService _genreContext;

        public EditModel(IMovieService context, IGenreService genreContext)
        {
            _context = context;
            _genreContext = genreContext;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GetProductListAsync(null) == null)
            {
                return NotFound();
            }

            var movie =  await _context.GetProductByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            Movie = movie.Data;
            ViewData["GenreId"] = new SelectList(_genreContext.GetCategoryListAsync().Result.Data, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(Movie).State = EntityState.Modified; 

            try
            {
               // await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
          return _context.GetProductByIdAsync(id).Result.Success;
        }
    }
}
