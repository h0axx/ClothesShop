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
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public string IdentityId { get; set; }
    }
}
