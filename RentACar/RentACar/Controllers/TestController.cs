using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RentACar.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFile file)
        {
            if(file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                ViewBag.Message = "File has been uploaded";
            }
            ViewBag.Message = "Could not upload file";
            return RedirectToAction("Index");
        }

        public ActionResult UploadFileJason()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile2()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                var serverPath = Server.MapPath("~/Uploads");
                var fullPath = Path.Combine(serverPath, fileName);
                file.SaveAs(fullPath);
            }
            var obj = new { status = true, filesUploaded = Request.Files.Count };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}