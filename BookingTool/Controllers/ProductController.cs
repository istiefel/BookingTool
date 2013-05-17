using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingTool.Models;

namespace BookingTool.Controllers
{
    [Authorize (Users = "info@crossvertise.com")]
    public class ProductController : Controller
    {
        BookingEntities bookingDb = new BookingEntities();
        
        //
        // GET: /Booking/ProductDetails
        public ActionResult ProductDetails()
        {
            var bookingContext = new BookingEntities();
            var productList = (from p in bookingContext.Products
                               select p).ToList();
            ViewBag.ProductList = productList;

            return View(productList);
        }


        //
        // GET: /Booking/ProductCreate
        public ActionResult ProductCreate()
        {
            ViewBag.ProductId = new SelectList(bookingDb.Products, "Id", "Name");
            return View();
        }

        //
        // POST: /Booking/ProductCreate
        [HttpPost]
        public ActionResult ProductCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                bookingDb.Products.Add(product);
                bookingDb.SaveChanges();
                return RedirectToAction("ProductDetails");
            }

            ViewBag.ProductId = new SelectList(bookingDb.Products, "Id", "Name", product.Id);
            return RedirectToAction("ProductDetails");
        }


        //
        // GET: /Booking/ProductEdit
        public ActionResult ProductEdit(int id)
        {
            Product product = bookingDb.Products.Find(id);
            ViewBag.ProductId = new SelectList(bookingDb.Products, "Id", "Name", product.Id);
            return View(product);
        }

        //
        // POST: /Booking/ProductEdit
        [HttpPost]
        public ActionResult ProductEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                bookingDb.Entry(product).State = EntityState.Modified;
                bookingDb.SaveChanges();
                return RedirectToAction("ProductDetails");
            }
            ViewBag.Id = new SelectList(bookingDb.Products, "Id", "Name", product.Id);
            return View(product);
        }

        //
        // GET: /Booking/ProductDelete
        public ActionResult ProductDelete(int id = 0)
        {
            Product product = bookingDb.Products.Find(id);
            return View(product);
        }

        //
        // POST: /Booking/ProductDelete
        [HttpPost, ActionName("ProductDelete")]
        public ActionResult ProductDeleteConfirmed(int id = 0)
        {
            Product product = bookingDb.Products.Find(id);
            bookingDb.Products.Remove(product);
            bookingDb.SaveChanges();
            return RedirectToAction("ProductDetails");
        }

        protected override void Dispose(bool disposing)
        {
            bookingDb.Dispose();
            base.Dispose(disposing);
        }
    }
}
