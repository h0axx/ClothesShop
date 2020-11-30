using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public class Photo
    {
        // Photo identification number
        [Required]
        public int Id { get; set; }
        // Path to file
        [Required]
        public string Path { get; set; }
        // Identification number of the product that photo belongs to
        [Required]
        public int ProductId { get; set; }

        public Photo(int productId,string path)
        {
            ProductId = productId;
            Path = path;
        }
    }
}
