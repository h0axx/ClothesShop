using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Account
{
    public class UserModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMemberData memberData;

        public IdentityUser LoggedUser { get; set; }
        [BindProperty]
        public Member UserData { get; set; }
        public string LoggedUserRole { get; set; }

        public UserModel(UserManager<IdentityUser> userManager,
                                IMemberData memberData)
        {
            this.userManager = userManager;
            this.memberData = memberData;
        }

        public async Task<IActionResult> OnGet()
        {
            return await RenderPage();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return await RenderPage();
            }

            LoggedUser = await userManager.GetUserAsync(User);

            if (LoggedUser == null)
            {
                return RedirectToPage("../Index");
            }

            memberData.Update(UserData);
            memberData.Commit();

            //await userManager.UpdateAsync(LoggedUser);

            return Page();

        }

        private async Task<IActionResult> RenderPage()
        {
            LoggedUser = await userManager.GetUserAsync(User);

            if (LoggedUser == null)
            {
                return RedirectToPage("../Index");
            }

            var roles = await userManager.GetRolesAsync(LoggedUser);
            LoggedUserRole = roles[0];

            /*User private details.*/
            UserData = memberData.GetMemberById(LoggedUser.Id);
            /*If there was an account created before private details database was delivered,
                temporarly create an instance of Member class.*/
            if (UserData == null)
            {
                UserData = new Member()
                {
                    IdentityId = LoggedUser.Id,
                };
                memberData.Add(UserData);
                memberData.Commit();
            }

            return Page();
        }
    }
}