using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Data
{
    public class OrderData : IOrderData
    {
        private readonly OnlineShopDbContext db;

        public OrderData(OnlineShopDbContext db)
        {
            this.db = db;
        }

        public Order Add(Order order)
        {
            db.Orders.Add(order);
            return order;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Order Delete(Order order)
        {
            db.Orders.Remove(order);
            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return db.Orders;
        }

        public int GetNewId()
        {
            return (db.Orders.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public Order GetOrderById(int orderId)
        {
            var query = db.Orders.Include("OrderedProduct").Where(z => z.Id == orderId).FirstOrDefault();

            return query;
        }

        public IEnumerable<Product> GetProductsFromOrder(int orderId)
        {
            var query = db.OrderedProducts.Where(z => z.OrderId == orderId);

            var products = new List<Product>();

            foreach(var item in query)
            {
                var product = db.Products.Where(p => p.Id == item.ProductId).FirstOrDefault();
                products.Add(product);
            }

            return products;
        }

        public Order Update(Order updatedOrder)
        {
            var entity = db.Attach(updatedOrder);
            entity.State = EntityState.Modified;
            return updatedOrder;
        }
    }
}
