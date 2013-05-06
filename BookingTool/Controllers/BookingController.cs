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
    [Authorize]
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
                                              orderby p.Booking.DateBookedUtc
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
            booking.DateBookedUtc = DateTime.UtcNow;
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
        public ActionResult Create(Booking booking)
        {
            booking.DateCreatedUtc = DateTime.UtcNow;
            //booking.DateBookedUtc = TimeZoneInfo.ConvertTimeToUtc(booking.DateBooked, MvcApplication.ApplicationTimeZoneInfo);

            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(booking);
                bookingDb.SaveChanges();
                return RedirectToAction("MyAccount");                
            }
            return View(booking);
        }

        public ActionResult Delete(int id = 0)
        {
            var booking = bookingDb.PartialBookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            if (ModelState.IsValid)
            {
                var booking = bookingDb.PartialBookings.Find(id);
                if (booking != null && booking.Sender == User.Identity.Name)
                {
                    bookingDb.PartialBookings.Remove(booking);
                    bookingDb.SaveChanges();
                    return RedirectToAction("MyAccount");
                }
            }
            return new HttpUnauthorizedResult();
        }

        protected override void Dispose(bool disposing)
        {
            bookingDb.Dispose();
            base.Dispose(disposing);
        }
    }
}
