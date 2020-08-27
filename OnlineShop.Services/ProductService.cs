using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Core;
using OnlineShop.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace OnlineShop.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductData productData;
        private readonly IHtmlHelper htmlHelper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductService(IProductData productData,
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
        public IEnumerable<Photo> ProductPhotos { get; set; }
        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        public Product OnGetEditPage(int? productId)
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

            return Product;
        }
        public Product OnPostEditPage(Product product, List<IFormFile> photos)
        {
            Product = product;
            Photos = photos;
            if (Photos != null && Photos.Count > 0)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                try
                {
                    ProductPhotos = productData.GetPhotosById(Product.Id);
                }
                catch (NullReferenceException)
                {
                }

                if (ProductPhotos != null)
                {
                    foreach (Photo photo in ProductPhotos)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", $"{Product.Id}", photo.Path);
                        File.Delete(filePath);
                    }
                }
            }
            if (Product.Id > 0)
            {
                if (Photos.Count > 0)
                {
                    Product.Photos = ProcessUploadedFile();
                    productData.DeletePhotos(Product.Id);
                    Product = productData.Update(Product);
                }
                else
                {
                    Product = productData.Update(Product);
                }
            }
            else
            {
                Product = productData.Add(Product);
                productData.Commit();
                Product.Id = productData.GetNewId() - 1;
                Product.Photos = ProcessUploadedFile();
                if (Product.Photos != null)
                {
                    Product = productData.Update(Product);
                }
            }
            productData.Commit();
            return Product;
        }
        private List<Photo> ProcessUploadedFile()
        {
            List<Photo> uniqueFilesNames = new List<Photo>();

            if (Photos != null)
            {
                int id;
                if (Product.Id == 0)
                {
                    id = productData.GetNewId();
                }
                else
                {
                    id = Product.Id;
                }
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", $"{id}");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                foreach (var photo in Photos)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                    var newPhoto = new Photo(Product.Id, uniqueFileName);
                    uniqueFilesNames.Add(newPhoto);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    photo.CopyTo(fileStream);
                }
            }

            return uniqueFilesNames;
        }
    }
}
