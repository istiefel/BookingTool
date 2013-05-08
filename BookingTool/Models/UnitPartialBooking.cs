using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    public class UnitPartialBooking
    {
        public UnitPartialBooking()
        {
            Unit = 1;
        }

        public int Id { get; set; }

        [DisplayName("Booking ID")]
        public int UnitBookingId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [HiddenInput(DisplayValue = true)]
        public string Sender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Recipient { get; set; }

        [Required]
        public decimal Unit { get; set; }

        public virtual UnitBooking UnitBooking { get; set; }

        public decimal GetComputedAmount(string userName)
        {
            if (userName == Recipient)
            {
                return -UnitBooking.TotalAmount;
            }
            else
            {
                return UnitBooking.TotalAmount;
            }
        }

        public string PersonOpposite(string userName)
        {
            if (userName == Recipient)
            {
                return Sender;
            }
            else
            {
                return Recipient;
            }
        }

        public PartialBooking ConvertToPartialBooking()
        {
            return new PartialBooking {Amount = Amount, BookingId = UnitBookingId, Id = Id, Recipient = Recipient, Sender = Sender};
        }

    }
}