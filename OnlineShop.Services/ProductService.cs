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

        public Product GetProduct(int? productId)
        {
            Sizes = htmlHelper.GetEnumSelectList<ClothingSize>();
            Types = htmlHelper.GetEnumSelectList<ClothingType>();
            Fabrics = htmlHelper.GetEnumSelectList<FabricType>();
            Genders = htmlHelper.GetEnumSelectList<GenderType>();

            SetProduct(productId);

            return Product;
        }

        public Product UpdateProduct(Product product, List<IFormFile> photos)
        {
            Product = product;
            Photos = photos;
            //If there are both new and old photos.
            DeletePhotos();
            //Add new product or update the changes of already existing one.
            ProcessProductChanges();

            return Product;
        }

        private void SetProduct(int? productId)
        {
            if (productId.HasValue)
            {
                Product = productData.GetById(productId.Value);
            }
            else
            {
                Product = new Product();
            }
        }
        private void ProcessProductChanges()
        {
            if (Product.Id > 0)
            {
                if (Photos.Count > 0)
                {
                    Product.Photos = ProcessUploadedFile(Product.Id);
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
                Product.Photos = ProcessUploadedFile(Product.Id);
            }
            productData.Commit();
        }
        private void DeletePhotos()
        {
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

                DeletePhotosFiles();
            }
        }
        private void DeletePhotosFiles()
        {
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
        private List<Photo> ProcessUploadedFile(int productId)
        {
            List<Photo> uniqueFilesNames = new List<Photo>();

            if (Photos != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", $"{productId}");
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

        public void SetProductsUnavailable(IEnumerable<Product> products)
        {
            foreach(var product in products)
            {
                product.Available = false;
                productData.Update(product);
            }
            productData.Commit();
        }
    }
}
