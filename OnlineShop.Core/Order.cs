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
            Cost = 0;
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public double Cost { get; set; }
        public OrderStatus Status { get; set; }
        public Delivery Delivery { get; set; }
        [Required]
        public List<OrderedProduct> Products { get; set; }
        
    }
}


