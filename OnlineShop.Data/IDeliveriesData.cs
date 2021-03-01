using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data
{
    public interface IDeliveriesData
    {
        Delivery Add(Delivery delivery);
        Delivery Delete(int id);
        IEnumerable<Delivery> GetDeliveries();
        Delivery GetDeliveryById(int id);
    }
}
