using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Models
{
    [Bind(Exclude = "DateCreatedUtc, DateBookedUtc")]
    public class ShareBooking : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [NotMapped]
        public decimal TotalAmount { get; set; }

        public DateTime DateBookedUtc { get; set; }

        [NotMapped]
        [DisplayName("Date Booked")]
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

        public virtual IList<SharePartialBooking> SharePartialBookings { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var field = new[] { "DateBooked" };

            if (DateBooked > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MvcApplication.ApplicationTimeZoneInfo))
            {
                yield return new ValidationResult("DateBooked kann nicht in der Zukunft liegen.", field);
            }
            if (DateBooked < DateTime.UtcNow.AddYears(-1))
            {
                yield return new ValidationResult("DateBooked kann nicht so weit in der Vergangenheit liegen.", field);
            }
        }

        public Booking ConvertToBooking()
        {
            var partialBookings = new List<PartialBooking>();
            foreach (var sharePartialBooking in SharePartialBookings)
            {
                sharePartialBooking.ShareBooking = this;
                partialBookings.Add(sharePartialBooking.ConvertToPartialBooking());
            }
            return new Booking
                       {
                           DateBooked = DateBooked,
                           DateBookedUtc = DateBookedUtc,
                           DateCreatedUtc = DateCreatedUtc,
                           Description = Description,
                           Id = Id,
                           Name = Name,
                           PartialBookings = partialBookings
                       };

        }
    }
}