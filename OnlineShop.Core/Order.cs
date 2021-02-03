using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public class Order
    {
        public Order()
        {
            Products = new List<OrderedProduct>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public List<OrderedProduct> Products { get; set; }
    }
}


