using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data
{
    public interface IOrderData
    {
        int GetNewId();
        int Commit();
        Order GetOrderById(int orderId);
        IEnumerable<Product> GetProductsFromOrder(int orderId);
        Order Add(Order order);
        Order Update(Order updatedOrder);
        Order Delete(Order order);
    }
}
