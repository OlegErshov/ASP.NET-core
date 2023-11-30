using Microsoft.AspNetCore.Mvc;
using WEB.Domain.Models;

namespace WEB.Components;

public class CartViewComponent : ViewComponent
{
    private readonly Cart _sessionCart;

    public CartViewComponent(Cart sessionCart)
    {
        _sessionCart = sessionCart;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult<IViewComponentResult>(View(_sessionCart));
    }
}