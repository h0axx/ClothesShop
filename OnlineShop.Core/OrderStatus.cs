using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.Core
{
    public enum OrderStatus
    {
        None,
        Canceled,
        Pending,
        [Display(Name = "In progress")]
        InProgress,
        Sent,
        Completed
    }
}
