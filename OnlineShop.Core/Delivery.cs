using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public class Delivery
    {
        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Provider { get; set; }
        [Required]
        [Range(0, 9999999.99)]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        public double Price { get; set; }
    }
}
