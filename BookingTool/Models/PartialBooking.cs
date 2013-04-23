using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    
    public class PartialBooking
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Bitte Email-Adresse des Senders eintragen.")]
        [DisplayName("Sender Email-Adresse")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Sender { get; set; }

        [Required(ErrorMessage = "Bitte Email-Adresse des Empfängers eintragen.")]
        [DisplayName("Empfänger Email-Adresse")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Recipient { get; set; }

        [Required(ErrorMessage = "Bitte Preis eintragen.")]
        [Range(0.01, 100.00, ErrorMessage = "Preis muss zwischen 0.01 und 100.00 liegen.")]
        [DisplayName("Teilsumme")]
        public decimal Amount { get; set; }

        [ScaffoldColumn(false)]
        public double Share { get; set; }

        public virtual Booking Booking { get; set; }
    }
}