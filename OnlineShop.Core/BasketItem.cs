using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Core
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MemberId { get; set; }
        public BasketItem(int memberId, int productId)
        {
            MemberId = memberId;
            ProductId = productId;
        }
    }
}
