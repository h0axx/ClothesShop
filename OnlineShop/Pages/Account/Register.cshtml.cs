using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;
using OnlineShop.Service;

namespace OnlineShop.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IMemberData memberData;

        [BindProperty]
        public RegisterInput userInput { get; set; }
        [TempData]
        public string Message { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                            SignInManager<IdentityUser> signInManager,
                            IEmailSender emailSender,
                            IMemberData memberData)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.memberData = memberData;
            userInput = new RegisterInput();
        }

        public IActionResult OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToPage("../Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser { UserName = userInput.Email, Email = userInput.Email};
            var result = await userManager.CreateAsync(user, userInput.Password);

            if (result.Succeeded)
            {
                //Token generation for email confirmation
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                //URL generating
                var confirmationLink = Url.Page("/Account/ConfirmEmail", null, new { userId = user.Id, token = token}, Request.Scheme);
                //Generating mail data
                var message = new Message(new string[] { user.Email }, "Confirm your email address.", confirmationLink);
                //Send autorization link
                emailSender.SendAutorizationEmail(message);

                //New member creation <- for user personal details
                var member = new Member()
                {
                    IdentityId = user.Id
                };
                memberData.Add(member);
                memberData.Commit();
                
                //Create customer role if not existing in DB
                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    var identityRole = new IdentityRole("Customer");
                    await roleManager.CreateAsync(identityRole);
                }
                //Add customer user to newly created user
                await userManager.AddToRoleAsync(user, "Customer");

                Message = "Confirm your email address to login.";

                return RedirectToPage("./Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return Page();
        }
    }
}