using OnlineShop.Core;
using System.Collections.Generic;

namespace OnlineShop.Data
{
    public interface IProductData
    {
        IEnumerable<Product> GetProductsByName(string name);
        IEnumerable<Product> GetProductsBy(string name, ClothingSize? size, GenderType? gender, FabricType? fabric, ClothingType? type);
        IEnumerable<Photo> GetPhotosById(int id);
        Product GetById(int id);
        int GetNewId();
        Product Update(Product updatedProduct);
        Product Add(Product product);
        Product Delete(int id);
        IEnumerable<Photo> DeletePhotos(int id);
        IEnumerable<BasketItem> DeleteBasketItems(int productId);
        int Commit();
    }
}
