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
            ViewBag.SubLocationId = new SelectList(db.Sublocations, "Id", "Title");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Title");
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Title");
            ViewBag.VehicleCategories = db.VehicleCategories.ToList();
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
            ViewBag.VehicleCategories = db.VehicleCategories.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateBookingRequest(BookingRequest model)
        {
            //db.BookingRequests.Add(model);
            //db.SaveChanges();
            //if(ModelState.IsValid)
            if (true)
            {
                try
                {
                    model.RequestInitiated = DateTime.Now;
                    model.RequestStatusId = db.RequestStatus.Where(rs => rs.Code == ApplicationWideData.RequestStatusNotViewed).First().Id;
                    db.BookingRequests.Add(model);
                    db.SaveChanges();
                    //return RedirectToAction("BookingRequestList");
                    return Json(new
                    {
                        status = "success",
                        exceptionOccurred = false,
                        data = new
                        {
                            bookingRequestId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
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
                    return Json(new
                    {
                        status = "success",
                        exceptionOccurred = false,
                        exceptionMessage = ex.Message,
                        data = errors
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    ViewBag.Errors = new List<string>
                    {
                        ex.Message
                    };
                    return Json(new
                    {
                        status = "error",
                        exceptionOccurred = true,
                        exceptionMessage = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            //ViewBag.SubLocationId = new SelectList(db.Sublocations, "Id", "Title", model.SubLocationId);
            //ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Title", model.PaymentMethodId);
            //ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title", model.VehicleId);
            //ViewBag.LocationId = new SelectList(db.Locations, "Id", "Title", model.LocationId);
            //ViewBag.VehicleCategories = db.VehicleCategories.ToList();
            //return View(model);
            return Json(new
            {
                status = "error",
                errorOccurred = false,
                data = "Model state is invalid"
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region VisitorQuery

        public ActionResult CreateVisitorQuery()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateVisitorQuery(VisitorQuery model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    model.DatePosted = DateTime.Now;
                    model.Status = ApplicationWideData.VisitorQueryNotViewed;
                    db.VisitorQueries.Add(model);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    

                    return Json(new
                    {
                        status = "success",
                        exceptionOccurred = false,
                        data = $"Visitor Query id: {model.Id}"
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    var errors = new List<string>();
                    foreach (var error in errorMessages)
                    {
                        errors.Add(error);
                    }
                    //ViewBag.VisitorQueryErrors = errors;
                    return Json(new
                    {
                        status = "error",
                        exceptionOccurred = true,
                        exceptionMessage = ex.Message,
                        data = errors
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var errors = new List<string>
                    {
                        ex.Message
                    };
                    return Json(new
                    {
                        status = "error",
                        exceptionOccurred = true,
                        exceptionMessage = ex.Message,
                        data = errors
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                status = "error",
                exceptionOccurred = false,
                data = "Model state is invalid"
            }, JsonRequestBehavior.AllowGet);
            //return View(model);

        }

        #endregion

        #region FareCalculator

        public ActionResult FareCalculator()
        {
            ViewBag.SubLocationId = new SelectList(db.Sublocations, "Id", "Title");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Title");
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Title");
            ViewBag.VehicleCategories = db.VehicleCategories.ToList();
            return View();
        }

        public JsonResult GetFareEstimate(DateTime requestDate, DateTime returnDate, int vehicleId, bool withDriver)
        {
            try
            {
                var vehicle = db.Vehicles.Find(vehicleId);
                if(vehicle == null)
                {
                    throw new Exception($"No vehicle found with id {vehicleId}");
                }
                if(requestDate > returnDate)
                {
                    throw new Exception("Request date must be earlier than return date.");
                }
                var numberOfDays = (returnDate - requestDate).Days + 1;
                var fare = 0M;
                if(numberOfDays <= ApplicationWideData.FareDaysStep1)
                {
                    fare = vehicle.Fare1 * numberOfDays;
                }
                else if(numberOfDays <= ApplicationWideData.FareDaysStep2)
                {
                    fare = vehicle.Fare2 * numberOfDays;
                }
                else
                {
                    fare = vehicle.Fare3 * numberOfDays;
                }
                if(withDriver)
                {
                    var driverCostStr = db.GlobalDatas.Where(gd => gd.Key.Equals(ApplicationWideData.DriverCostKey, StringComparison.OrdinalIgnoreCase)).Select(gd => gd.Value).First();
                    var driverCost = Convert.ToDecimal(driverCostStr);
                    fare = fare + driverCost * numberOfDays;
                }
                return Json(new
                {
                    status = "success",
                    exceptionOccurred = false,
                    noOfDays = numberOfDays,
                    fare = fare
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    exceptionOccurred = true,
                    exceptionMessage = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Others

        public JsonResult GetVehiclesInCategory(int categoryId)
        {
            try
            {
                var category = db.VehicleCategories.Find(categoryId);
                if(category == null)
                {
                    throw new Exception("Could not find the category.");
                }
                var vehicles = db.Vehicles.Where(v => v.CategoryId == categoryId).Select(v => new { id = v.Id, title = v.Title }).ToArray();
                return Json(new
                {
                    status = "success",
                    exceptionOccurred = false,
                    vehicleCount = vehicles.GetLength(0),
                    vehicles = vehicles
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    exceptionOccurred = true,
                    exceptionMessage = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}