using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;

namespace OnlineShop.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        [BindProperty]
        public UserInput userInput { get; set; }

        public RegisterModel(UserManager<User> userManager,
                             RoleManager<IdentityRole> roleManager,
                            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            userInput = new UserInput();
        }

        public class UserInput
        {
            [Required]
            [EmailAddress]
            [BindProperty]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [BindProperty]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [BindProperty]
            [Compare(nameof(Password),
                ErrorMessage = "Password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User { UserName = userInput.Email, Email = userInput.Email };
            var result = await userManager.CreateAsync(user, userInput.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("../Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return Page();
        }
    }
}