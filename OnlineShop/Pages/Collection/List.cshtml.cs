using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Collection
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IProductData productData;
        public IEnumerable<Product> Products { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public ListModel(IConfiguration config, IProductData productData)
        {
            this.config = config;
            this.productData = productData;
        }
        public void OnGet()
        {
            Products = productData.GetProductsByName(SearchTerm);
        }
    }
}