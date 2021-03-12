using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;

namespace OnlineShop.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public LoginInput userInput { get; set; }
        [TempData]
        public string Message { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager,
                                        UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public IActionResult OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToPage("../Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await userManager.FindByEmailAsync(userInput.Email);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You need to confirm your email. Check your mailbox.");
                return Page();
            }
            var result = await signInManager.PasswordSignInAsync(userInput.Email, userInput.Password,userInput.RememberMe, lockoutOnFailure: false);
            // If login is successful, redirect to index page
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToPage("../Index");
                }
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}