using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace OnlineShop.Data
{
    public class SqlProductData : IProductData
    {
        private readonly OnlineShopDbContext db;

        public SqlProductData(OnlineShopDbContext db)
        {
            this.db = db;
        }
        public Product Add(Product product)
        {
            db.Products.Add(product);
            return product;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Product Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                db.Products.Remove(product);
            }
            return product;
        }

        public IEnumerable<Photo> DeletePhotos(int id)
        {
            var photos = GetPhotosById(id);
            if (photos.Count() > 0)
            {
                foreach (var photo in photos)
                {
                    db.Photos.Remove(photo);
                }
            }
            return photos;
        }

        public Product GetById(int id)
        {
            var query = db.Products.Include("Photos").Where(z => z.Id == id).FirstOrDefault();

            return query;
        }

        public int GetNewId()
        {
            return (db.Products.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public IEnumerable<Photo> GetPhotosById(int id)
        {
            var query = from p in db.Photos
                        where p.ProductId == id
                        select p;
            return query;
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            var query = db.Products.Include("Photos");

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(z => z.Name.StartsWith(name));
            }

            //var query = from p in db.Products
            //            where p.Name.StartsWith(name) || string.IsNullOrEmpty(name)
            //            orderby p.Name
            //            select p;

            return query;
        }

        public Product Update(Product updatedProduct)
        {
            var entity = db.Attach(updatedProduct);
            entity.State = EntityState.Modified;
            return updatedProduct;
        }
        public Product UpdateWithoutPhotos(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
