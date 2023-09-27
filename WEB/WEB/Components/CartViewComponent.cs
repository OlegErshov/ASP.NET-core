using Microsoft.AspNetCore.Mvc;

namespace WEB.Components;

//[ViewComponent]
public class CartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(double price)
    {
        return View();
    }
}