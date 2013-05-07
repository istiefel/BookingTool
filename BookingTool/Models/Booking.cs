using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    [Bind(Exclude = "DateCreatedUtc, DateBookedUtc")]
    public class Booking : IValidatableObject
    {
        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Beschreibung")]
        public string Description { get; set; }

        
        //[UIHint("GermanDate")]
        public DateTime DateBookedUtc { get; set; }

        [NotMapped]
        [DisplayName("Buchungsdatum")]
        public DateTime DateBooked
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateBookedUtc, MvcApplication.ApplicationTimeZoneInfo); } 
            set
            {
                if (value.Kind == DateTimeKind.Utc)
                    DateBookedUtc = value;
                else 
                    DateBookedUtc = TimeZoneInfo.ConvertTimeToUtc(value, MvcApplication.ApplicationTimeZoneInfo);
            }
        }

        [DisplayName("Date Created")]
        //[UIHint("GermanDate")]
        public DateTime DateCreatedUtc { get; set; }

        public virtual IList<PartialBooking> PartialBookings { get; set; } 

        public IEnumerable<ValidationResult>Validate(ValidationContext validationContext)
        {
            var field = new[] {"DateBooked"};

            if (DateBooked > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MvcApplication.ApplicationTimeZoneInfo))
            {
                yield return new ValidationResult("DateBooked kann nicht in der Zukunft liegen.", field);
            }
            if (DateBooked < DateTime.UtcNow.AddYears(-1))
            {
                yield return new ValidationResult("DateBooked kann nicht so weit in der Vergangenheit liegen.", field);
            }
        }
    
        [NotMapped]
        public decimal TotalAmount {
            get {
                if (PartialBookings == null)
                {
                    return 0;
                }
                else
                {
                    return PartialBookings.Sum(p => p.Amount);
                }
                //return (PartialBookings == null) ? 0 : PartialBookings.Sum(p => p.Amount);
            }
        }   
    }
}