﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingTool.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /
        public ActionResult Index()
        {
            ViewBag.Message = "Finanzen verwalten.";

            return View();
        }
    }
}
