using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public class Member
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string City { get; set; }
        public string Address { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public string IdentityId { get; set; }      
        public List<BasketItem> Basket { get; set; }

        public Member()
        {
            Basket = new List<BasketItem>();
        }
    }
}
