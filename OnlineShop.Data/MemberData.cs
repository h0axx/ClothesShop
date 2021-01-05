using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Data
{
    public class MemberData : IMemberData
    {
        private readonly OnlineShopDbContext db;

        public MemberData(OnlineShopDbContext db)
        {
            this.db = db;
        }
        public Member Add(Member member)
        {
            db.Members.Add(member);
            return member;
        }

        public BasketItem AddToBasket(int memberId, int productId)
        {
            var basketItem = new BasketItem(memberId, productId);
            var check = db.BasketItems.Where(z => z.MemberId == memberId && z.ProductId == productId).FirstOrDefault();
            if (check == null)
            {
                db.BasketItems.Add(basketItem);
                return basketItem;
            }
            else
            {
                return null;
            }
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Member Delete(string identityId)
        {
            var member = GetMemberById(identityId);
            db.Members.Remove(member);
            return member;
        }

        public BasketItem DeleteFromBasket(int itemId)
        {
            var basketItem = GetBasketItem(itemId);
            db.BasketItems.Remove(basketItem);
            return basketItem;
        }

        public IEnumerable<BasketItem> GetAllBasketItems(int memberId)
        {
            var allBasketItems = from i in db.BasketItems
                                 where i.MemberId == memberId
                                 select i;
            return allBasketItems;
        }

        public BasketItem GetBasketItem(int itemId)
        {
            var query = db.BasketItems.Where(z => z.Id == itemId).FirstOrDefault();
            return query;
        }

        public Member GetMemberById(string identityId)
        {
            var query = db.Members.Where(z => z.IdentityId == identityId).FirstOrDefault();
            return query;
        }

        public IEnumerable<Member> GetMembers()
        {
            var query = db.Members;
            return query;
        }

        public Member Update(Member updatedMember)
        {
            var entity = db.Members.Attach(updatedMember);
            entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return updatedMember;
        }
    }
}
