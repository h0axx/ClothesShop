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
