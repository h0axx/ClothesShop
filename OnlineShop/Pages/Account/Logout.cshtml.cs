using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;

namespace OnlineShop.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Login");
            }
            await signInManager.SignOutAsync();
            return RedirectToPage("../Index");
        }
    }
}
