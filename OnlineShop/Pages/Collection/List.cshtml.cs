using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IHtmlHelper htmlHelper;

        public IEnumerable<Product> Products { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public ClothingSize Size { get; set; }
        [BindProperty(SupportsGet = true)]
        public GenderType Gender { get; set; }
        [BindProperty(SupportsGet = true)]
        public FabricType Fabric { get; set; }
        [BindProperty(SupportsGet = true)]
        public ClothingType Type { get; set; }

        public IEnumerable<SelectListItem> Sizes { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Fabrics { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        
        [TempData]
        public string Message { get; set; }
        public bool Admin { get; set; }

        public ListModel(IConfiguration config, IProductData productData, 
                        SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
                        IHtmlHelper htmlHelper)
        {
            this.config = config;
            this.productData = productData;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.htmlHelper = htmlHelper;
        }
        public void OnGet()
        {
            Sizes = htmlHelper.GetEnumSelectList<ClothingSize>();
            Types = htmlHelper.GetEnumSelectList<ClothingType>();
            Fabrics = htmlHelper.GetEnumSelectList<FabricType>();
            Genders = htmlHelper.GetEnumSelectList<GenderType>();

            Products = productData.GetProductsBy(SearchTerm, Size, Gender, Fabric, Type);

            if (IsUserAdmin())
            {
                Admin = true;
            }
            else
            {
                Admin = false;
            }
        }

        private bool IsUserAdmin()
        {

            if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                return true;
            }

            return false;
        }
    }
}