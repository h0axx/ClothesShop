﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Core;
using System.Collections.Generic;

namespace OnlineShop.Service
{
    public interface IProductService
    {
        IEnumerable<SelectListItem> Sizes { get; }
        IEnumerable<SelectListItem> Types { get; }
        IEnumerable<SelectListItem> Fabrics { get; }
        IEnumerable<SelectListItem> Genders { get; }
        Product UpdateProduct(Product product, List<IFormFile> photos);
        Product GetProduct(int? productId);
        void SetProductsUnavailable(IEnumerable<Product> products);
    }
}
