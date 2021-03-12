using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Account.Admin.Deliveries
{
    public class AddModel : PageModel
    {
        private readonly IDeliveriesData deliveryData;

        [BindProperty]
        public Delivery Delivery { get; set; }
        public AddModel(IDeliveriesData deliveryData)
        {
            this.deliveryData = deliveryData;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            deliveryData.Add(Delivery);
            deliveryData.Commit();

            return Page();
        }
    }
}