using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookingTool.Models;
using WebMatrix.WebData;

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
        // GET: /Booking/
        public ActionResult MyAccount()
        {
            var accountOverview = new AccountOverview();
            accountOverview.UserName = User.Identity.Name;

            accountOverview.PartialBookings = (from p in bookingDb.PartialBookings
                                              where p.Sender == accountOverview.UserName || p.Recipient == accountOverview.UserName
                                              select p).ToList();
            return View(accountOverview);
        }


        //
        // GET: /Booking/Create
        [Authorize]
        public ActionResult Create(int? participantsCount)
        {
            if (participantsCount == null)
                return View("EnterParticipantsCount");

            ViewBag.ParticipantsCount = participantsCount;

            var userContext = new UsersContext();
            var userNames = from u in userContext.UserProfiles
                            where u.UserName != User.Identity.Name
                            select u.UserName;
            ViewBag.UserNames = userNames.ToList();

            //var userNames = new UsersContext().UserProfiles
            //    .Where(u => u.UserName != User.Identity.Name)
            //    .Select(u => u.UserName )
            //    .ToList();
            //ViewBag.UserNames = userNames.ToList();

            var booking = new Booking();
            booking.DateBooked = DateTime.Now;
            booking.PartialBookings = new List<PartialBooking>();

            for (var i = 0; i < participantsCount; i++)
            { 
                booking.PartialBookings.Add(new PartialBooking{ Sender = User.Identity.Name });
            }

            return View(booking);
        }


        //
        // POST: /Booking/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Booking booking)
        {
            booking.DateCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(booking);
                bookingDb.SaveChanges();
                return RedirectToAction("MyAccount");                
            }
            return View(booking);
        }
    }
}
