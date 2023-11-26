using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WEB.IdentityServer.Models;

namespace WEB.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        public AvatarController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> manager) 
        {
            _environment = webHostEnvironment;
            _userManager = manager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAvatar()
        {
            var id = _userManager.GetUserId(User);
            if(User is null)
            {
                return NotFound();
            }

            FileExtensionContentTypeProvider provaider = new();

            var path = Path.Combine(_environment.ContentRootPath, "wwwroot", "Images",$"{id}.jpg");

            provaider.TryGetContentType(path, out string contentType);

            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, contentType);
            }

           var defPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "Images", "efron.jpg");

            provaider.TryGetContentType(defPath, out string defCntentType);
            return PhysicalFile(defPath, defCntentType);
        }
    }
}
