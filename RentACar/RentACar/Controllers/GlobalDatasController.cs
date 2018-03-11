using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class GlobalDatasController : Controller
    {
        private Entities db = new Entities();

        // GET: GlobalDatas
        public ActionResult Index()
        {
            return View(db.GlobalDatas.ToList());
        }

        // GET: GlobalDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalData globalData = db.GlobalDatas.Find(id);
            if (globalData == null)
            {
                return HttpNotFound();
            }
            return View(globalData);
        }

        // GET: GlobalDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GlobalDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Key,Value")] GlobalData globalData)
        {
            if (ModelState.IsValid)
            {
                db.GlobalDatas.Add(globalData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(globalData);
        }

        // GET: GlobalDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalData globalData = db.GlobalDatas.Find(id);
            if (globalData == null)
            {
                return HttpNotFound();
            }
            return View(globalData);
        }

        // POST: GlobalDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,Value")] GlobalData globalData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(globalData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(globalData);
        }

        // GET: GlobalDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalData globalData = db.GlobalDatas.Find(id);
            if (globalData == null)
            {
                return HttpNotFound();
            }
            return View(globalData);
        }

        // POST: GlobalDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GlobalData globalData = db.GlobalDatas.Find(id);
            db.GlobalDatas.Remove(globalData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
