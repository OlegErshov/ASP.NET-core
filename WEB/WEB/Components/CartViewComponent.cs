using Microsoft.AspNetCore.Mvc;

namespace WEB.Components
{
    [ViewComponent]
    public class CartViewComponent 
    {
        public string Invoke(double price)
        {
            return price.ToString();
        }
    }
}