using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Collection
{
    public class EditModel : PageModel
    {
        private readonly IProductData productData;
        private readonly IHtmlHelper htmlHelper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditModel(IProductData productData,
                        IHtmlHelper htmlHelper,
                        IWebHostEnvironment webHostEnvironment)
        {
            this.productData = productData;
            this.htmlHelper = htmlHelper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<SelectListItem> Sizes { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Fabrics { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        [BindProperty]
        public Product Product { get; set; }
        public List<string> ProductPhotosPaths { get; set; }
        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        public IActionResult OnGet(int? productId)
        {
            Sizes = htmlHelper.GetEnumSelectList<ClothingSize>();
            Types = htmlHelper.GetEnumSelectList<ClothingType>();
            Fabrics = htmlHelper.GetEnumSelectList<FabricType>();
            Genders = htmlHelper.GetEnumSelectList<GenderType>();

            if (productId.HasValue)
            {
                Product = productData.GetById(productId.Value);
            }
            else
            {
                Product = new Product();
            }

            if (Product == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Sizes = htmlHelper.GetEnumSelectList<ClothingSize>();
                Types = htmlHelper.GetEnumSelectList<ClothingType>();
                Fabrics = htmlHelper.GetEnumSelectList<FabricType>();
                Genders = htmlHelper.GetEnumSelectList<GenderType>();
                return Page();
            }
            if (Photos != null && Photos.Count > 0)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                ProductPhotosPaths = productData.GetById(Product.Id).PhotosPath;

                if (ProductPhotosPaths != null)
                {
                    foreach (string path in ProductPhotosPaths)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", path);
                        System.IO.File.Delete(filePath);
                    }
                }
                Product.PhotosPath = ProcessUploadedFile();
            }
            if (Product.Id > 0)
            {
                if (Photos.Count > 0)
                {
                    Product = productData.Update(Product);
                }
                else
                {
                    Product = productData.UpdateWithoutPhotos(Product);
                }
            }
            else
            {
                Product = productData.Add(Product);
            }
            productData.Commit();
            TempData["Message"] = "Product saved!";
            return RedirectToPage("./Detail", new { productId = Product.Id });
        }
        private List<string> ProcessUploadedFile()
        {
            List<string> uniqueFilesNames = new List<string>();

            if (Photos != null)
            {
                foreach (var photo in Photos)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                    uniqueFilesNames.Add(uniqueFileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    photo.CopyTo(fileStream);
                }
            }

            return uniqueFilesNames;
        }
    }
}