using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Collection
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private IProductData productData;
        private readonly IOrderData orderData;
        private IWebHostEnvironment webHostEnvironment;

        public Product Product { get; set; }

        public DeleteModel(IProductData productData, IOrderData orderData, IWebHostEnvironment webHostEnvironment)
        {
            this.productData = productData;
            this.orderData = orderData;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult OnGet(int productId)
        {
            Product = productData.GetById(productId);
            if (Product == null)
            {
                return RedirectToPage("./List");
            }
            return Page();
        }
        public IActionResult OnPost(int productId)
        {
            var product = productData.Delete(productId);
            if (product == null)
            {
                return RedirectToPage("./NotFound");
            }
            var photos = productData.GetPhotosById(productId);
            if (photos != null)
            { 
                foreach (Photo photo in photos)
                {
                    string filePhotoPath = Path.Combine(webHostEnvironment.WebRootPath,
                    "images", $"{product.Id}", photo.Path);
                    System.IO.File.Delete(filePhotoPath);
                }
            }
            productData.Commit();

            TempData["Message"] = $"Product {product.Name} has been successfully deleted.";
            return RedirectToPage("./List");
        }
    }
}