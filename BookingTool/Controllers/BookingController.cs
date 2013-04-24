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
        // GET: /Booking/Create
        public ActionResult Create()
        {
            return View(new PartialBooking());
        }

       
        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(PartialBooking partialBooking)
        {
            if (ModelState.IsValid)
            {
                bookingDb.PartialBookings.Add(partialBooking);
                bookingDb.SaveChanges();
                return RedirectToAction("Index");
            }
    
            ViewBag.BookingId = new SelectList(bookingDb.PartialBookings, "BookingId", "Name");
            return View(partialBooking);
        }
    
    }
}
