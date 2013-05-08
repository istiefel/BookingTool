using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookingTool.Models
{
    public class BookingEntities : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PartialBooking> PartialBookings { get; set; }
        public DbSet<Product> Products { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }


    }
}