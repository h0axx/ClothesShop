using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public class Photo
    {
        public Photo(int productId,string path)
        {
            ProductId = productId;
            Path = path;
        }
        [Required]
        public string Path { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
