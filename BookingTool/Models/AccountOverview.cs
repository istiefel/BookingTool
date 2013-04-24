using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    public class AccountOverview
    {
        public string User { get; set; }
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

        public virtual Booking Booking { get; set; }
    }
}