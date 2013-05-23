using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    public class AccountOverview
    {
        [DisplayName("Benutzername")]
        public string UserName { get; set; }

        public virtual List<PartialBooking> PartialBookings { get; set; }

        public string FilterPerson { get; set; }

        [DisplayName("Summe")]
        public decimal TotalAmount
        {
            get
            {
                if (PartialBookings == null)
                {
                    return 0;
                }
                else 
                return Debit + Credit;
            } 
        }

        [DisplayName("Schulden")]
        public decimal Debit
        {
            get
            {
                if (PartialBookings == null)
                {
                    return 0;
                }
                else
                {
                    return (-1)*PartialBookings.Where(p => p.Recipient == UserName && p.DatePaidUtc == null).Sum(p => p.Amount);
                }
            }
        }

        [DisplayName("Guthaben")]
        public decimal Credit
        {
            get
            {
                if (PartialBookings == null)
                {
                    return 0;
                }
                else
                {
                    return PartialBookings.Where(p => p.Sender == UserName && p.DatePaidUtc == null).Sum(p => p.Amount);
                }
            }
        }
    }
}