﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Core;
using OnlineShop.Data;
using OnlineShop.Service;

namespace OnlineShop.Pages.Collection
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IProductService productService;
        public IEnumerable<SelectListItem> Sizes { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Fabrics { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        [BindProperty]
        public Product Product { get; set; }
        public IEnumerable<Photo> ProductPhotos { get; set; }
        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        public EditModel(IProductService productService)
        {
            this.productService = productService;
        }
        public IActionResult OnGet(int? productId)
        {
            Product = productService.GetProduct(productId);

            Sizes = productService.Sizes;
            Types = productService.Types;
            Fabrics = productService.Fabrics;
            Genders = productService.Genders;

            if (Product == null)
            {
                return RedirectToPage("./NotFound");
            }
            else
            {
                return Page();
            }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Edit", new { productId = Product.Id });
            }
            Product = productService.UpdateProduct(Product, Photos);
            TempData["Message"] = "Product saved!";
            return RedirectToPage("./Detail", new { productId = Product.Id });
        }
    }
}