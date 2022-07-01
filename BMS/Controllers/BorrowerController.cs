using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BMS.Models;
using EntityState = System.Data.Entity.EntityState;

namespace BMS.Controllers
{
    public class BorrowerController : Controller
    {
        private BMSDATAEntities db = new BMSDATAEntities();

        // GET: Borrower
        public ActionResult Index()
        {
            return View(db.Borrowers.ToList());
        }

        // GET: Borrower/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrower borrower = db.Borrowers.Find(id);
            if (borrower == null)
            {
                return HttpNotFound();
            }
            return View(borrower);
        }

        // GET: Borrower/Create
        public ActionResult Create()
        {
            Borrower borrower = new Borrower();
            return View(borrower);
        }

        // POST: Borrower/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                if (borrower.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(borrower.ImageUpload.FileName);
                    string extension = Path.GetExtension(borrower.ImageUpload.FileName);
                    filename = filename + extension;
                    borrower.borrower_image = "~/Images/Borrower/" + filename;
                    borrower.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Borrower/Book/"), filename));
                }
                db.Borrowers.Add(borrower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(borrower);
        }

        // GET: Borrower/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrower borrower = db.Borrowers.Find(id);
            if (borrower == null)
            {
                return HttpNotFound();
            }
            return View(borrower);
        }

        // POST: Borrower/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                if (borrower.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(borrower.ImageUpload.FileName);
                    string extension = Path.GetExtension(borrower.ImageUpload.FileName);
                    filename = filename + extension;
                    borrower.borrower_image = "~/Images/Borrower/" + filename;
                    borrower.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/Borrower/"), filename));
                }
                db.Entry(borrower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            };
            return View(borrower);
        }

        // GET: Borrower/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrower borrower = db.Borrowers.Find(id);
            if (borrower == null)
            {
                return HttpNotFound();
            }
            return View(borrower);
        }

        // POST: Borrower/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrower borrower = db.Borrowers.Find(id);
            db.Borrowers.Remove(borrower);
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
