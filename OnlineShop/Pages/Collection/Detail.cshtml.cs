using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Collection
{
    public class DetailModel : PageModel
    {
        private readonly IProductData productData;
        public Product Product { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        [TempData]
        public string Message { get; set; }
        
        public DetailModel(IProductData productData)
        {
            this.productData = productData;
        }
        public IActionResult OnGet(int productId)
        {
            Product = productData.GetById(productId);
            Photos = productData.GetPhotosById(productId);
           
            if (Product == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}