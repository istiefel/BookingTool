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

        public ActionResult MyAccount()
        {
            var accountOverview = new AccountOverview();
            accountOverview.User = "i.stiefel@crossvertise.com";

            accountOverview.PartialBookings = (from p in bookingDb.PartialBookings
                                              where p.Sender == accountOverview.User || p.Recipient == accountOverview.User
                                              select p).ToList();


            return View(accountOverview);
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
            return View(booking);
        }

        //
        // POST: /Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            booking.DateCreated = DateTime.Now;
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
