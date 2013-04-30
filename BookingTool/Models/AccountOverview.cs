using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    public class AccountOverview
    {
        public string UserName { get; set; }
        public virtual IList<PartialBooking> PartialBookings { get; set; }
        public decimal TotalAmount
        {
            get
            {
                if (PartialBookings == null)
                {
                    return 0;
                }
                else 
                return PartialBookings.Sum(p => p.Amount);
            } 
        }

    }
}