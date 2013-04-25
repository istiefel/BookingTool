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
    public class Booking : IValidatableObject
    {
        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public DateTime DateBooked { get; set; }

        //[HiddenInput] 
        public DateTime DateCreated { get; set; }

        public IEnumerable<ValidationResult>Validate(ValidationContext validationContext)
        {
            if (DateBooked > DateTime.Now)
            {
                yield return new ValidationResult("DateBooked kann nicht in der Zukunft liegen.");
            }
            if (DateBooked < DateTime.Now.AddYears(-1))
            {
                yield return new ValidationResult("DateBooked kann nicht so weit in der Vergangenheit liegen.");
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
  
        public virtual IList<PartialBooking> PartialBookings { get; set; } 

    }
}