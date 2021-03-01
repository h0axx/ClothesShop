using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Data
{
    public class DeliveryData : IDeliveriesData
    {
        private readonly OnlineShopDbContext db;

        public DeliveryData(OnlineShopDbContext db)
        {
            this.db = db;
        }
        public Delivery Add(Delivery delivery)
        {
            db.Add(delivery);
            return delivery;
        }

        public Delivery Delete(int id)
        {
            var delivery = GetDeliveryById(id);

            if (delivery != null)
            {
                db.Remove(delivery);
            }

            return delivery;
        }

        public IEnumerable<Delivery> GetDeliveries()
        {
            return db.Deliveries;
        }

        public Delivery GetDeliveryById(int id)
        {
            return db.Deliveries.Where(z => z.Id == id).FirstOrDefault();
        }
    }
}
