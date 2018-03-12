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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

        private Entities db = new Entities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        #region VehicleCategory

        public ActionResult VehicleCategoryList()
        {
            return View(db.VehicleCategories.AsEnumerable());
        }

        public ActionResult CreateVehicleCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVehicleCategory(VehicleCategory model)
        {
            if(ModelState.IsValid)
            {
                db.VehicleCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("VehicleCategoryList");
            }
            return View(model);
        }

        public ActionResult DeleteVehicleCategory(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.VehicleCategories.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteVehicleCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVehicleCategoryConfirmed(int id)
        {
            var model = db.VehicleCategories.Find(id);
            db.VehicleCategories.Remove(model);
            db.SaveChanges();
            return RedirectToAction("VehicleCategoryList");
        }

        #endregion

        #region Vehicle

        public ActionResult VehicleList()
        {
            return View(db.Vehicles.AsEnumerable());
        }

        public ActionResult CreateVehicle()
        {
            ViewBag.CategoryId = new SelectList(db.VehicleCategories, "Id", "Title");
            ViewBag.TransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVehicle(Vehicle model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Vehicles.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("VehicleList");
                }
                catch(DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var errors = new List<string>();
                    foreach (var error in errorMessages)
                    {
                        errors.Add(error);
                    }
                    ViewBag.Errors = errors;
                }
                catch(Exception ex)
                {
                    ViewBag.Errors = new List<string>
                    {
                        ex.Message
                    };
                }
            }
            ViewBag.CategoryId = new SelectList(db.VehicleCategories, "Id", "Title", model.CategoryId);
            ViewBag.TransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Title", model.TransmissionId);
            return View(model);
        }

        public ActionResult EditVehicle(int id)
        {
            var model = db.Vehicles.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.CategoryId = new SelectList(db.VehicleCategories, "Id", "Title", model.CategoryId);
            ViewBag.TransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Title", model.TransmissionId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVehicle(Vehicle model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("VehicleList");
                }
                catch(DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var errors = new List<string>();
                    foreach (var error in errorMessages)
                    {
                        errors.Add(error);
                    }
                    ViewBag.Errors = errors;
                }
                catch(Exception ex)
                {
                    ViewBag.Errors = new List<string>
                    {
                        ex.Message
                    };
                }
            }
            ViewBag.CategoryId = new SelectList(db.VehicleCategories, "Id", "Title", model.CategoryId);
            ViewBag.TransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Title", model.TransmissionId);
            return View(model);
        }

        public ActionResult DeleteVehicle(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Vehicles.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteVehicle")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVehicleConfirmed(int id)
        {
            var model = db.Vehicles.Find(id);
            db.Vehicles.Remove(model);
            db.SaveChanges();
            return RedirectToAction("VehicleList");
        }

        #endregion

        #region Location

        public ActionResult LocationList()
        {
            return View(db.Locations.AsEnumerable());
        }

        public ActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLocation(Location model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Locations.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("LocationList");
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

        public ActionResult DeleteLocation(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Locations.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteLocation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLocationConfirmed(int id)
        {
            var model = db.Locations.Find(id);
            db.Locations.Remove(model);
            db.SaveChanges();
            return RedirectToAction("LocationList");
        }

        #endregion

        #region SubLocation

        public ActionResult SublocationList()
        {
            return View(db.Sublocations.AsEnumerable());
        }

        public ActionResult CreateSublocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSublocation(Sublocation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Sublocations.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("SublocationList");
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

        public ActionResult DeleteSublocation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Sublocations.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteSublocation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSublocationConfirmed(int id)
        {
            var model = db.Sublocations.Find(id);
            db.Sublocations.Remove(model);
            db.SaveChanges();
            return RedirectToAction("SublocationList");
        }

        #endregion

        #region Location

        public ActionResult PaymentMethodList()
        {
            return View(db.PaymentMethods.AsEnumerable());
        }

        public ActionResult CreatePaymentMethod()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePaymentMethod(PaymentMethod model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PaymentMethods.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("PaymentMethodList");
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

        public ActionResult DeletePaymentMethod(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.PaymentMethods.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeletePaymentMethod")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePaymentMethodConfirmed(int id)
        {
            var model = db.PaymentMethods.Find(id);
            db.PaymentMethods.Remove(model);
            db.SaveChanges();
            return RedirectToAction("PaymentMethod");
        }

        #endregion

        #region Booking

        public ActionResult BookingList()
        {
            return View(db.Bookings.AsEnumerable());
        }

        public ActionResult CreateBooking()
        {
            ViewBag.BookingRequestId = new SelectList(db.BookingRequests, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking(Booking model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Bookings.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("BookingList");
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
            ViewBag.BookingRequestId = new SelectList(db.BookingRequests, "Id", "Email", model.BookingRequestId);
            return View(model);
        }

        public ActionResult EditBooking(int id)
        {
            var model = db.Bookings.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.BookingRequestId = new SelectList(db.BookingRequests, "Id", "Email", model.BookingRequestId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(Booking model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("BookingList");
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
            ViewBag.BookingRequestId = new SelectList(db.BookingRequests, "Id", "Email", model.BookingRequestId);
            return View(model);
        }

        #endregion

        #region VisitorQuery

        public ActionResult VisitorQueryList()
        {
            return View(db.VisitorQueries.AsEnumerable());
        }

        public ActionResult VisitorQueryDetails(int id)
        {
            var model = db.VisitorQueries.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }
        
        public JsonResult EditVisitorQuery(int id)
        {
            var model = db.VisitorQueries.Find(id);
            if(model == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            model.Status = ApplicationWideData.VisitorQueryViewed;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        } 

        public ActionResult DeleteVisitorQuery(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.VisitorQueries.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("DeleteVisitorQuery")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVisitorQueryConfirmed(int id)
        {
            var model = db.VisitorQueries.Find(id);
            db.VisitorQueries.Remove(model);
            db.SaveChanges();
            return RedirectToAction("VisitorQueryList");
        }

        #endregion
    }
}