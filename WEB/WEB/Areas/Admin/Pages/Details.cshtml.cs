using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using WEB.Domain.Entities;
using WEB.Services.MovieServices;

namespace WEB.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IMovieService _context;

        public DetailsModel(IMovieService context)
        {
            _context = context;
        }

      public Movie Movie { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GetProductListAsync(null) == null)
            {
                return NotFound();
            }

            var movie = await _context.GetProductByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            else 
            {
                Movie = movie.Data;
            }
            return Page();
        }
    }
}
