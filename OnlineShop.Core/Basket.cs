using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Core
{
    public class Basket
    {
        public string UserId { get; set; }
        public List<Product> Products { get; set; }

        public Basket()
        {
            Products = new List<Product>();
        }
    }
}
