using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using WEB.Domain.Entities;
using WEB.Domain.Models;
using WEB.Extensions;

namespace WEB.Services.CartServices
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }
        public override void AddToCart(Movie product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }

        public override void Remove(int id)
        {
            base.Remove(id);
            Session?.Set("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
