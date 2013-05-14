using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    public class SharePartialBooking
    {
        public SharePartialBooking()
        {
            WeightFactor = 1;
        }

        public int Id { get; set; }

        [DisplayName("Booking ID")]
        public int ShareBookingId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [HiddenInput(DisplayValue = true)]
        public string Sender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Recipient { get; set; }

        [Required]
        public decimal WeightFactor { get; set; }

        public virtual ShareBooking ShareBooking { get; set; }

        public decimal Amount
        {
            get
            {
                if (ShareBooking == null)
                {
                    return 0;
                }
                else
                {
                    {
                        var totalAmount = ShareBooking.TotalAmount;
                        var sumWeightFactor = ShareBooking.SharePartialBookings.Sum(s => s.WeightFactor);
                        return totalAmount/sumWeightFactor*WeightFactor;
                    }
                }
            }
        }

        public PartialBooking ConvertToPartialBooking()
        {
            return new PartialBooking {Amount = Amount, BookingId = ShareBookingId, Id = Id, Recipient = Recipient, Sender = Sender};
        }
    }
}