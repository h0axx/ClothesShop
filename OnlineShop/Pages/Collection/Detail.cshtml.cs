using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Collection
{
    public class DetailModel : PageModel
    {
        private readonly IProductData productData;
        private readonly IMemberData memberData;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public Product Product { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        [TempData]
        public string Message { get; set; }
        
        public DetailModel(IProductData productData, IMemberData memberData,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.productData = productData;
            this.memberData = memberData;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
        public async Task<IActionResult> OnPost(int productId)
        {
            Product = productData.GetById(productId);
            Photos = productData.GetPhotosById(productId);
            var loggedUser = await userManager.GetUserAsync(User);

            if (Product == null)
            {
                return RedirectToPage("./NotFound");
            }
            else if (loggedUser == null)
            {
                return RedirectToPage("../Account/Login");
            }
            else if (!Product.Available)
            {
                TempData["Message"] = "Product unavailable!";
                return RedirectToPage("./Detail");
            }
            else
            {
                var memberId = memberData.GetMemberById(loggedUser.Id).Id;
                var result = memberData.AddToBasket(memberId, productId);
                if (result == null)
                {
                    TempData["Message"] = "Product already in basket!";
                    return RedirectToPage("./Detail");
                }
                TempData["Message"] = "Product added to basket!";
                memberData.Commit();
                return RedirectToPage("./Detail");
            }
        }
    }
}