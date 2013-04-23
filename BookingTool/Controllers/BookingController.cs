using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingTool.Models;

namespace BookingTool.Controllers
{
    private BookingEntities bookingDb = new BookingEntities();

    public class BookingController : Controller
    {
        //
        // GET: /Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            return View(booking);
        }
    }
}
