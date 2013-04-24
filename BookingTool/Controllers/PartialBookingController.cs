using BookingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Controllers
{
    public class PartialBookingController : Controller
    {
        //
        // GET: /PartialBooking/

        BookingEntities bookingDb = new BookingEntities();

        public ActionResult Index()
        {
            var model = bookingDb.PartialBookings;
            return View(model);
        }
    }
}
