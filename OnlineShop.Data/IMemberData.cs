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
        int Commit();
    }
}
