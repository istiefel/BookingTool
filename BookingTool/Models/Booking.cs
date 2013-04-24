using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    public class Booking
    {
        
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateBooked { get; set; }
        public System.DateTime DateCreated { get; set; }

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