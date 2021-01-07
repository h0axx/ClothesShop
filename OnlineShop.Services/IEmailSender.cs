using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Service
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
