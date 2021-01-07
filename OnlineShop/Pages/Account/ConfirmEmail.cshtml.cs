using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineShop.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        [TempData]
        public string Message { get; set; }

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string userId, string token)
        {

            if (userId == null || token == null)
            {
                return RedirectToPage("./NotFound");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                Message = "User Id is invalid";
                return RedirectToPage("./NotFound");
            }
            if (user.EmailConfirmed == true)
            {
                return RedirectToPage("../Index");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Page();
            }
            else
            {
                Message = "Email cannot be confirmed";
                return RedirectToPage("../Error");
            }
        }
    }
}