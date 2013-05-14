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

        public decimal Amount
        {
            get
            {
                if (UnitBooking == null || UnitBooking.Product == null)
                {
                    return 0;
                }
                else
                {
                    var price = UnitBooking.Product.Price;
                    return Unit * price;
                }        
            }
        }

        [Required]
        public int Unit { get; set; }

        public virtual UnitBooking UnitBooking { get; set; }

        public PartialBooking ConvertToPartialBooking()
        {
            return new PartialBooking {BookingId = UnitBookingId, Amount = Amount, Id = Id, Recipient = Recipient, Sender = Sender};
        }

    }
}