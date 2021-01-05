using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data
{
    public interface IMemberData
    {
        IEnumerable<Member> GetMembers();
        Member GetMemberById(string identityId);
        Member Update(Member updatedMember);
        Member Add(Member member);
        Member Delete(string identityId);
        BasketItem GetBasketItem(int itemId);
        IEnumerable<BasketItem> GetAllBasketItems(int memberId);
        BasketItem AddToBasket(int memberId, int productId);
        BasketItem DeleteFromBasket(int itemId);
        int Commit();
    }
}
