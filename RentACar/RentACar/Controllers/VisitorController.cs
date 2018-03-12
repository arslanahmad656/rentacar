using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using RentACar.Models;
using System.Net.Http;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace RentACar.Controllers
{
    public class VisitorController : Controller
    {
        private Entities db = new Entities();

        // GET: Visitor
        public ActionResult Index()
        {
            return View();
        }

        #region BookingRequest

        public ActionResult BookingRequestList()
        {
            return View(db.BookingRequests.AsEnumerable());
        }

        public ActionResult CreateBookingRequest()
        {
            ViewBag.SubLocationId = new SelectList(db.Sublocations, "Id", "Title");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Title");
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBookingRequest(BookingRequest model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    model.RequestInitiated = DateTime.Now;
                    model.RequestStatusId = db.RequestStatus.Where(rs => rs.Code == ApplicationWideData.RequestStatusNotViewed).First().Id;
                    db.BookingRequests.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("BookingRequestList");
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var errors = new List<string>();
                    foreach (var error in errorMessages)
                    {
                        errors.Add(error);
                    }
                    ViewBag.Errors = errors;
                }
                catch (Exception ex)
                {
                    ViewBag.Errors = new List<string>
                    {
                        ex.Message
                    };
                }
            }
            ViewBag.SubLocationId = new SelectList(db.Sublocations, "Id", "Title", model.SubLocationId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Title", model.PaymentMethodId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title", model.VehicleId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Title", model.LocationId);
            return View(model);
        }

        #endregion

        #region VisitorQuery

        public ActionResult CreateVisitorQuery()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVisitorQuery(VisitorQuery model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    model.DatePosted = DateTime.Now;
                    model.Status = ApplicationWideData.VisitorQueryNotViewed;
                    db.VisitorQueries.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var errors = new List<string>();
                    foreach (var error in errorMessages)
                    {
                        errors.Add(error);
                    }
                    ViewBag.Errors = errors;
                }
                catch (Exception ex)
                {
                    ViewBag.Errors = new List<string>
                    {
                        ex.Message
                    };
                }
            }
            return View(model);
        }

        #endregion
    }
}