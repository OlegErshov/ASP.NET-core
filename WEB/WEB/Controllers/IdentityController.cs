using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB.Controllers
{
    
    public class IdentityController : ControllerBase
    {
        public async Task Login()
        {
            await HttpContext.ChallengeAsync("oidc",
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                });
        }

        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("cookie");
            await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
        }
    }
}

