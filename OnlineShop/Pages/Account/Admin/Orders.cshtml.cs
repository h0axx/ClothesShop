using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Account.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly IOrderData orderData;

        public List<Order> Orders { get; set; }

        public OrdersModel(IOrderData orderData)
        {
            this.orderData = orderData;
        }
        public void OnGet()
        {
            Orders = orderData.GetAllOrders().ToList();
        }
    }
}