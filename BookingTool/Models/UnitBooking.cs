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
    public class UnitBooking : IValidatableObject
    {
        public int Id { get; set; }

        public virtual IList<UnitPartialBooking> UnitPartialBookings { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [NotMapped]
        [DisplayName("Summe")]
        public decimal TotalAmount
        {
            get
            {
                if (Product == null || UnitPartialBookings == null)
                {
                    return 0;
                }
                var price = Product.Price;
                var sumUnit = UnitPartialBookings.Sum(s => s.Unit);
                return sumUnit*price;
            }
        }

        public DateTime DateBookedUtc { get; set; }

        [NotMapped]
        [DisplayName("Buchungsdatum")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
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

            foreach (var unitPartialBooking in UnitPartialBookings)
            {
                unitPartialBooking.UnitBooking = this;
                partialBookings.Add(unitPartialBooking.ConvertToPartialBooking());
            }
            return new Booking
            {
                DateBooked = DateBooked,
                DateBookedUtc = DateBookedUtc,
                DateCreatedUtc = DateCreatedUtc,
                Id = Id,
                Name = Product.Name,
                Description = Product.Name,
                PartialBookings = partialBookings
            };
        }

    }
}