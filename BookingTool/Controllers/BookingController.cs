using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
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
        public ActionResult Create(int? participantsCount, string method)
        {
            if (participantsCount == null)
                return View("EnterParticipantsCount");

            if (method == "byAmount")
            {
                return RedirectToAction("CreateByAmount", new { participantsCount });
            }
            else if (method == "byShares")
            {
                return RedirectToAction("CreateByShares", new { participantsCount });
            }
            else if (method == "byUnit")
            {
                return RedirectToAction("CreateByUnit", new {participantsCount});
            }
            return new HttpStatusCodeResult(400);
         }


        //
        // GET: /Booking/CreateByAmount
        public ActionResult CreateByAmount(int participantsCount)
        {
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
                booking.PartialBookings.Add(new PartialBooking { Sender = User.Identity.Name });
            }
            return View(booking);
        }

        //
        // POST: /Booking/CreateByAmount
        [HttpPost]
        public ActionResult CreateByAmount(Booking booking)
        {
            booking.DateCreatedUtc = DateTime.UtcNow;
            //booking.DateBookedUtc = TimeZoneInfo.ConvertTimeToUtc(booking.DateBooked, MvcApplication.ApplicationTimeZoneInfo);

            if (booking.PartialBookings.Any(a => a.Amount < 0))
            {
                ModelState.AddModelError("Amount", "Betrag is negative");
            }

            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(booking);
                bookingDb.SaveChanges();
                return RedirectToAction("MyAccount");                
            }

            ViewBag.ParticipantsCount = booking.PartialBookings.Count();

            var userContext = new UsersContext();
            var userNames = from u in userContext.UserProfiles
                            where u.UserName != User.Identity.Name
                            select u.UserName;
            ViewBag.UserNames = userNames.ToList();

            return View(booking);
        }



        // GET: /Booking/CreateByShares
        public ActionResult CreateByShares(int participantsCount)
        {
            ViewBag.ParticipantsCount = participantsCount;

            var userContext = new UsersContext();
            var userName = from u in userContext.UserProfiles
                           select u.UserName;
            ViewBag.UserNames = userName.ToList();

            var shareBooking = new ShareBooking();
            shareBooking.DateBookedUtc = DateTime.UtcNow;
            shareBooking.SharePartialBookings = new List<SharePartialBooking>();

            for (var i = 0; i < participantsCount; i++)
            {
                shareBooking.SharePartialBookings.Add(new SharePartialBooking { Sender = User.Identity.Name });
            }
            return View(shareBooking);
        }

        // 
        // POST: /Booking/CreateByShares
        [HttpPost]
        public ActionResult CreateByShares(ShareBooking shareBooking)
        {
            shareBooking.DateCreatedUtc = DateTime.UtcNow;

            if (shareBooking.TotalAmount < 0)
            {
                ModelState.AddModelError("TotalAmount", "Summe is negative");
            }

            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(shareBooking.ConvertToBooking());
                bookingDb.SaveChanges();
                return RedirectToAction("MyAccount");
            }

            ViewBag.ParticipantsCount = shareBooking.SharePartialBookings.Count();

            var userContext = new UsersContext();
            var userName = from u in userContext.UserProfiles
                           select u.UserName;
            ViewBag.UserNames = userName.ToList();

            return View(shareBooking);
        }


        // 
        // GET: /Booking/CreateByUnit
        [Authorize(Users = "info@crossvertise.com")]
        public ActionResult CreateByUnit(int participantsCount)
        {
            ViewBag.ParticipantsCount = participantsCount;

            var userContext = new UsersContext();
            var userName = from u in userContext.UserProfiles
                           where u.UserName != User.Identity.Name
                           select u.UserName;
            ViewBag.UserNames = userName.ToList();

            var unitBooking = new UnitBooking();
            unitBooking.DateBookedUtc = DateTime.UtcNow;
            unitBooking.UnitPartialBookings = new List<UnitPartialBooking>();

            var bookingContext = new BookingEntities();
            var productList = (from p in bookingContext.Products
                               select p).ToDictionary(k => k.Id, v => v.Name + " (" + v.Price + ")");
            ViewBag.ProductList = productList;

            for (var i = 0; i < participantsCount; i++)
            {
                unitBooking.UnitPartialBookings.Add(new UnitPartialBooking { Sender = User.Identity.Name });
            }
            return View(unitBooking);
        }

        //
        // POST: /Booking/CreateByUnit
        [HttpPost]
        [Authorize(Users = "info@crossvertise.com")]
        public ActionResult CreateByUnit(UnitBooking unitBooking)
        {
            var bookingContext = new BookingEntities();
        
            unitBooking.Product = (from p in bookingContext.Products
                                  where p.Id == unitBooking.ProductId
                                  select p).SingleOrDefault();

            foreach (var unitPartialBooking in unitBooking.UnitPartialBookings)
            {
                unitPartialBooking.UnitBooking = unitBooking;
            }

            //unitBooking.Product = bookingContext.Products.SingleOrDefault(p => p.Id == unitBooking.ProductId);
            //unitBooking.Product = bookingContext.Products.Find(unitBooking.ProductId);
            if (unitBooking.Product == null)
            {
                return HttpNotFound();
            }

            unitBooking.DateCreatedUtc = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                bookingDb.Bookings.Add(unitBooking.ConvertToBooking());
                bookingDb.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(unitBooking);
        }


        //
        // GET: /Booking/Delete
        //[Authorize(Users = "info@crossvertise.com")]
        public ActionResult Delete(int id = 0)
        {
            var booking = bookingDb.PartialBookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //
        // POST: /Booking/Delete
        [HttpPost, ActionName("Delete")]
        //[Authorize(Users = "info@crossvertise.com")]
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


        //
        // GET: /Booking/FilterPerson
        public ActionResult FilterPerson(string person)
        {
            var accountOverview = new AccountOverview();
            accountOverview.UserName = User.Identity.Name;
            accountOverview.FilterPerson = person;

            accountOverview.PartialBookings = (from p in bookingDb.PartialBookings
                                               where (p.Sender == person || p.Sender == accountOverview.UserName) && (p.Recipient == person || p.Recipient == accountOverview.UserName)
                                               select p).ToList();
            return View(accountOverview);
        }


        //
        // GET: /Booking/MarkAsPaid
        public ActionResult MarkAsPaid(int id = 0)
        {
            var booking = bookingDb.PartialBookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //
        // POST: /Booking/MarkAsPaid
        [HttpPost, ActionName("MarkAsPaid")]
        public ActionResult MarkAsPaidConfirmed(int id = 0)
        {
            if (ModelState.IsValid)
            {
                var partialBooking = bookingDb.PartialBookings.Find(id);


                if (partialBooking != null && partialBooking.Sender == User.Identity.Name && partialBooking.DatePaidUtc == null)
                {
                    partialBooking.DatePaidUtc = DateTime.UtcNow;
                    bookingDb.SaveChanges();
                    return RedirectToAction("MyAccount");
                }
            }
            return new HttpUnauthorizedResult();
        }


        //
        // GET: /Booking/MultipleAsPaid
        public ActionResult MultipleAsPaid(string userName)
        {
            var accountOverview = new AccountOverview();
            accountOverview.UserName = User.Identity.Name;
            accountOverview.FilterPerson = userName;

            accountOverview.PartialBookings = (from p in bookingDb.PartialBookings
                                               where (p.Sender == userName || p.Sender == accountOverview.UserName) && (p.Recipient == userName || p.Recipient == accountOverview.UserName) && p.DatePaidUtc == null
                                               select p).ToList();
            return View(accountOverview);
        }

        //
        // POST: /Booking/MultipleAsPaid
        [HttpPost, ActionName("MultipleAsPaid")]
        public ActionResult MultipleAsPaidConfirmed(string userName)
        {
            if (ModelState.IsValid)
            {
                var partialBookings = (from p in bookingDb.PartialBookings
                                       where (p.Sender == userName || p.Sender == User.Identity.Name) && (p.Recipient == userName || p.Recipient == User.Identity.Name) && p.DatePaidUtc == null
                                       select p).ToList();

                foreach (var booking in partialBookings)
                {
                    booking.DatePaidUtc = DateTime.UtcNow;
                }
                bookingDb.SaveChanges();
                return RedirectToAction("FilterPerson", new {person = userName});
            }
            return new HttpUnauthorizedResult();
        }


        //
        // GET: /Booking/SendEmail
        public ActionResult SendEmail(string to)
        {
            var accountOverview = new AccountOverview();
            accountOverview.UserName = User.Identity.Name;
            accountOverview.FilterPerson = to;

            accountOverview.PartialBookings = (from p in bookingDb.PartialBookings
                                               where(p.Sender == to || p.Sender == accountOverview.UserName) &&
                                                    (p.Recipient == to || p.Recipient == accountOverview.UserName)
                                               select p).ToList();

            new MailController().PaymentReminder(accountOverview).Deliver();

            return RedirectToAction("FilterPerson", new {person = to});
        }
    }
}
