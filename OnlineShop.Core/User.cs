using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Core
{
    public class User : IdentityUser
    {
        [Required, StringLength(20), Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, StringLength(20)]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
    }
}
