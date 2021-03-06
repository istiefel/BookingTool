﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace BookingTool.Models
{

    public class PartialBooking
    {
        public int Id { get; set; }

        public DateTime? DatePaidUtc { get; set; }

        [DisplayName("Nr.")]
        public int BookingId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [HiddenInput(DisplayValue = true)]
        public string Sender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Recipient { get; set; }

        [Required]
        //[Range(0.01, 100.00)]
        [DisplayName("Betrag")]
        public decimal Amount { get; set; }

        public virtual Booking Booking { get; set; }

        public decimal GetComputedAmount(string userName)
        {
            if (userName == Recipient)
            {
                return -Amount;
            }
            else
            {
                return Amount;
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
    }
}
