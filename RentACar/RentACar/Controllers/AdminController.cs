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
using System.IO;

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
                    if(model.Fare1 >= model.Fare2)
                    {
                        throw new Exception("Fare 1 must be lesser than Fare 2");
                    }
                    if(model.Fare1 >= model.Fare3)
                    {
                        throw new Exception("Fare 1 must be lesser than Fare 3");
                    }
                    if (model.Fare2 >= model.Fare3)
                    {
                        throw new Exception("Fare 2 must be lesser than Fare 3");
                    }
                    if(model.Year < ApplicationWideData.VehicleModelLowerLimt)
                    {
                        throw new Exception($"Vehicle model must be at least {ApplicationWideData.VehicleModelLowerLimt}");
                    }
                    if (model.Year > ApplicationWideData.VehicleModelUpperLimt)
                    {
                        throw new Exception($"Vehicle model must be at max {ApplicationWideData.VehicleModelUpperLimt}");
                    }
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
            var images = db.VehicleImages.Where(vi => vi.VehicleId == model.Id).Select(vi => vi.Image).ToList();
            //db.Vehicles.Remove(model);
            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            
            images.ForEach(i => db.Images.Remove(i));
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

        #region VehicleImages

        public ActionResult ImageDashBoard()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title");
            return View();
        }

        public ActionResult AddVehicleImage()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Title");
            return View();
        }

        [HttpPost]
        public JsonResult UploadVehicleImages()
        {
            try
            {
                var vehicleId = Convert.ToInt32(Request.Form["vehicleId"]);
                var dbVehicle = db.Vehicles.Find(vehicleId);
                if(dbVehicle == null)
                {
                    throw new Exception("Could not find the vehicle");
                }
                var totalImages = Convert.ToInt32(Request.Form["imgCount"]);
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var dbImage = new Image()
                    {
                        Path = "something"
                    };
                    db.Images.Add(dbImage);
                    db.SaveChanges();
                    var img = Request.Files[i];
                    var imgExtension = Path.GetExtension(img.FileName);
                    var imgNameToStoreInDb = $"img_veh_{vehicleId}_{dbImage.Id}{imgExtension}";
                    var rootPath = Server.MapPath(ApplicationWideData.StorageRootPath);
                    var serverPath = Path.Combine(rootPath, imgNameToStoreInDb);
                    img.SaveAs(serverPath);
                    dbImage = db.Images.Find(dbImage.Id);
                    dbImage.Path = imgNameToStoreInDb;
                    db.Entry(dbImage).State = EntityState.Modified;
                    db.SaveChanges();
                    var vehicleImage = new VehicleImage
                    {
                        ImageId = dbImage.Id,
                        VehicleId = dbVehicle.Id
                    };
                    db.VehicleImages.Add(vehicleImage);
                    db.SaveChanges();
                }
                var statusObj = new
                {
                    status = "success",
                    exceptionOccurred = "false",
                    filesUploaded = Request.Files.Count
                };
                return Json(statusObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var statusObj = new
                {
                    status = "error",
                    exceptionOccurred = "true",
                    exceptionMessage = ex.Message
                };
                return Json(statusObj, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetImagesForVehicle(int vehicleId)
        {
            try
            {
                var vehicle = db.Vehicles.Find(vehicleId);
                if(vehicle == null)
                {
                    throw new Exception("Could not find the image");
                }
                var imagePaths = db.VehicleImages
                    .Where(vi => vi.VehicleId == vehicle.Id)
                    .Select(vi => vi.Image)
                    .Select(i => new
                    {
                        id = i.Id,
                        path = i.Path
                    })
                    .ToArray();
                var statusObj = new
                {
                    status = "success",
                    exceptionOccurred = false,
                    pathArray = imagePaths
                };
                return Json(statusObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var statusObj = new
                {
                    status = "error",
                    exceptionOccurred = true,
                    exceptionMessage = ex.Message
                };
                return Json(statusObj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteImageForVehicle()
        {
            try
            {
                var vehicleId = Convert.ToInt32(Request.Form["vehicleId"]);
                var imageId = Convert.ToInt32(Request.Form["imageId"]);
                var vehicle = db.Vehicles.Find(vehicleId);
                if (vehicle == null)
                {
                    throw new Exception("Vehicle not found.");
                }
                var img = db.Images.Find(imageId);
                if(img == null)
                {
                    throw new Exception("Image not found");
                }
                var fileToDelete = Path.Combine(Server.MapPath(ApplicationWideData.StorageRootPath), img.Path);
                System.IO.File.Delete(fileToDelete);
                var vehicleImg = db.VehicleImages.Where(vi => vi.VehicleId == vehicle.Id && vi.ImageId == img.Id)
                    .First();
                db.VehicleImages.Remove(vehicleImg);
                db.SaveChanges();
                db.Images.Remove(img);
                db.SaveChanges();
                return Json(new
                {
                    status = "success",
                    exceptionOccurred = false
                }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
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

        #region BookingRequest

        public ActionResult BookingRequestList()
        {
            return View(db.BookingRequests);
        }

        public ActionResult BookingRequestDetails(int id)
        {
            var model = db.BookingRequests.Find(id);
            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult DeleteBookingRequest(int id)
        {
            var model = db.BookingRequests.Find(id);
            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteBookingRequest")]
        public ActionResult DeleteBookingRequestConfirmed(int id)
        {
            var model = db.BookingRequests.Find(id);
            db.BookingRequests.Remove(model);
            db.SaveChanges();
            return RedirectToAction("BookingRequestList");
        }

        [HttpPost]
        public JsonResult ChangeBookingRequestRequestViewDate(int id, string antiForgeryToken)
        {
            var model = db.BookingRequests.Find(id);
            if(model == null)
            {
                throw new Exception("HttpNotFound-404");
            }
            model.RequestViewed = DateTime.Now;
            model.RequestStatusId = db.RequestStatus.Where(rs => rs.Code == ApplicationWideData.RequestStatusNoAction).First().Id;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true);
        }

        [HttpPost]
        public JsonResult ApproveBookingRequest(int id, string antiForgeryToken)
        {
            var model = db.BookingRequests.Find(id);
            if (model == null)
            {
                throw new Exception("HttpNotFound-404");
            }
            model.RequestStatusId = db.RequestStatus.Where(rs => rs.Code == ApplicationWideData.RequestStatusApproved).First().Id;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true);
        }

        [HttpPost]
        public JsonResult CancelBookingRequest(int id, string antiForgeryToken)
        {
            var model = db.BookingRequests.Find(id);
            if (model == null)
            {
                throw new Exception("HttpNotFound-404");
            }
            model.RequestStatusId = db.RequestStatus.Where(rs => rs.Code == ApplicationWideData.RequestStatusCancelled).First().Id;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true);
        }

        #endregion
    }
}