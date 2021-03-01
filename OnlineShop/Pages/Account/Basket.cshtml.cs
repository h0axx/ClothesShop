using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;
using OnlineShop.Service;

namespace OnlineShop.Pages.Account
{
    [Authorize]
    public class BasketModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMemberData memberData;
        private readonly IProductData productData;
        private readonly IOrderData orderData;
        private readonly IProductService productService;

        public IdentityUser LoggedUser { get; set; }
        public Member UserData { get; set; }
        public List<Product> BasketItems { get; set; }
        public List<Product> UnavilableItems { get; set; }
        public List<int> ItemsId { get; set; }
        public int ItemIdToDelete { get; set; }
        public double TotalCost { get
            {
                double temp = 0;
                foreach(var item in BasketItems)
                {
                    temp += item.Price;
                }
                return temp;
            } }
        [TempData]
        public string Message { get; set; }

        public BasketModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager
                        , IMemberData memberData, IProductData productData, IOrderData orderData, IProductService productService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.memberData = memberData;
            this.productData = productData;
            this.orderData = orderData;
            this.productService = productService;
        }
        public async Task<IActionResult> OnGet()
        {
            LoggedUser = await userManager.GetUserAsync(User);
            UserData = memberData.GetMemberById(LoggedUser.Id);
            UserData.Basket = memberData.GetAllBasketItems(UserData.Id).ToList();

            //BasketItems - list with products, ItemIds - list with id's of basket items, UnavailableItems - list with unavailable products in basket
            SetListsBasedOnBasket(UserData.Basket);

            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int itemId)
        {
            LoggedUser = await userManager.GetUserAsync(User);
            UserData = memberData.GetMemberById(LoggedUser.Id);
            var item = memberData.GetBasketItem(itemId);

            if (item != null)
            {
                if (UserData.Id == item.MemberId)
                {
                    memberData.DeleteFromBasket(itemId);
                    memberData.Commit();
                    TempData["Message"] = "Item deleted!";
                    return RedirectToPage("./Basket");
                }
                else
                {
                    TempData["Message"] = "Nice try! You can not delete this item!";
                    return RedirectToPage("./Basket");
                }
            }
            else
            {
                TempData["Message"] = "Item is not existing";
                return RedirectToPage("./Basket");
            }
        }

        public async Task<IActionResult> OnPostRelizeOrder()
        {
            LoggedUser = await userManager.GetUserAsync(User);
            UserData = memberData.GetMemberById(LoggedUser.Id);
            UserData.Basket = memberData.GetAllBasketItems(UserData.Id).ToList();

            SetListsBasedOnBasket(UserData.Basket);

            if (BasketItems.Count > 0)
            {
                //Creation of new order
                var order = new Order();
                //Timestamp
                order.Time = DateTime.Now;
                //Setting a member id of whom the order belong to
                order.MemberId = UserData.Id;
                //Setting order.Products to products that are in basket list.
                order.Products = OrderedProducts(BasketItems);

                foreach(var orderedProduct in order.Products)
                {
                    order.Cost += productData.GetById(orderedProduct.ProductId).Price;
                }

                //Setting products from user basket to unavailable to make them impossible to buy twice or add to another basket
                productService.SetProductsUnavailable(BasketItems);

                //Adding data into DB  
                orderData.Add(order);
                orderData.Commit();

                TempData["Message"] = "Order placed";
            }
            else
            {
                TempData["Message"] = "Order cannot be placed";
            }

            return RedirectToPage("./Basket");
        }

        private void SetListsBasedOnBasket(IEnumerable<BasketItem> basketItems)
        {
            BasketItems = new List<Product>();
            ItemsId = new List<int>();
            UnavilableItems = new List<Product>();
            Product product;

            foreach (var item in basketItems)
            {
                product = productData.GetById(item.ProductId);

                if (product.Available)
                {
                    BasketItems.Add(product);
                    ItemsId.Add(item.Id);
                }
                else
                {
                    UnavilableItems.Add(product);
                }
            }
        }

        private List<OrderedProduct> OrderedProducts(IEnumerable<Product> products)
        {
            var orderedProducts = new List<OrderedProduct>();

            foreach(var product in products)
            {
                var orderedProduct = new OrderedProduct(product.Id);
                orderedProducts.Add(orderedProduct);
            }

            return orderedProducts;
        }
    }
}