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

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Sender { 
            get {return ich;}
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Recipient { get; set; }

        [Required]
        [Range(0.01, 100.00)]
        public decimal Amount { get; set; }

        [Required]
        public double Share { get; set; }

        public virtual Booking Booking { get; set; }
    }
}