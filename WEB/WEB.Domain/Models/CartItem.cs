using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB.Domain.Entities;

namespace WEB.Domain.Models
{
    public class CartItem
    {
        public Movie Item { get; set; }
        public int Count { get; set; }
    }
}
