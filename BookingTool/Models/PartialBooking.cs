using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    
    public class PartialBooking
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public decimal Amount { get; set; }
        public double Share { get; set; }
        public virtual Booking Booking { get; set; }
    }
}