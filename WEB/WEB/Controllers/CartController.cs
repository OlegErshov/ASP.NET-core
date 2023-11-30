using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB.Domain.Models;
using WEB.Extensions;
using WEB.Services.CartServices;
using WEB.Services.MovieServices;

namespace WEB.Controllers
{
    
    public class CartController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly Cart _cart;
        public CartController(IMovieService movieService, Cart cart)
        {
            _movieService = movieService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _movieService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }

        public async Task<ActionResult> Delete(int id, string returnUrl)
        {
            var data = await _movieService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart.Remove(id);
            }
            return Redirect(returnUrl);
        }

    }
}
