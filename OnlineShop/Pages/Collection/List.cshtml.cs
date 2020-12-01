using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public IEnumerable<Product> Products { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [TempData]
        public string Message { get; set; }
        public bool Admin { get; set; }
        public ListModel(IConfiguration config, IProductData productData, 
                        SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.config = config;
            this.productData = productData;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            Products = productData.GetProductsByName(SearchTerm);

            if (IsUserAdmin().Result)
            {
                Admin = true;
            }
            else
            {
                Admin = false;
            }
        }

        private async Task<bool> IsUserAdmin()
        {
            var loggedUser = await userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return false;
            }

            var userRoles = await userManager.GetRolesAsync(loggedUser);

            var userRole = userRoles[0];

            if(userRole == "Admin")
            {
                return true;
            }

            return false;
        }
    }
}