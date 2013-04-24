using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingTool.Models;

namespace BookingTool.Controllers
{
    public class BookingController : Controller
    {
        BookingEntities bookingDb = new BookingEntities();

        //
        // GET: /Booking/
        public ActionResult Index()
        {
            var model = bookingDb.Bookings;
            return View(model);
        }

        //
        // GET: /Booking/Create
        public ActionResult Create(int? participantsCount)
        {
            if (participantsCount == null)
                return View("EnterParticipantsCount");


            ViewBag.ParticipantsCount = participantsCount;

            var booking = new Booking();
            booking.DateBooked = DateTime.Now;
            booking.DateCreated = DateTime.Now;
            return View(booking);
        }

        //
        // POST: /Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(booking);
                bookingDb.SaveChanges();
                return RedirectToAction("Index");                
            }
            return View(booking);
        }
    }
}
