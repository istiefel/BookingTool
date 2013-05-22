using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net;
using ActionMailer.Net.Mvc;
using System.Net;
using System.Net.Mail;
using BookingTool.Models;

namespace BookingTool.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult PaymentReminder(AccountOverview accountOverview)
        {
            To.Add(accountOverview.FilterPerson);
            From = accountOverview.UserName;
            Subject = "Erinnerung";
            return Email("PaymentReminder", accountOverview);
        }
    }

}
