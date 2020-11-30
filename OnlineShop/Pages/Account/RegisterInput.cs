using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Pages.Account
{
        public class RegisterInput
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
}