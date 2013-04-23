using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    public class Booking
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte Vornamen eintragen.")]
        [DisplayName("Vorname")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bitte Nachnamen eintragen.")]
        [DisplayName("Nachname")]
        [StringLength(160)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bitte Datum der Buchung eintragen.")]
        [ScaffoldColumn(false)]
        public DateTime DateBooked { get; set; }
        
        [ScaffoldColumn(false)]
        public System.DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Bitte Preis eintragen.")]
        [DisplayName("Summe")]
        [ScaffoldColumn(false)]
        public decimal TotalAmount { get; set; }
        
        public ICollection<PartialBooking> PartialBookings { get; set; } 

    }
}