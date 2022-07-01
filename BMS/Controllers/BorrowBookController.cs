using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BMS.Models;
using EntityState = System.Data.Entity.EntityState;

namespace BMS.Controllers
{
    public class BorrowBookController : Controller
    {
        private BMSDATAEntities db = new BMSDATAEntities();
        
        private void CheckExpire()
        {
            var listExpire = db.BorrowBooks.Where(p=>p.status.Equals("Progress")).ToList();
            foreach(var book in listExpire)
            {

                //var result = DateTime.Now.Subtract( book.expire_date??DateTime.Now);
                var result = DateTime.Now.Ticks - book.expire_date.Value.Ticks;
                if (result > 0)
                {
                    book.status = "Late";
                    db.SaveChanges();
                }
            }
        }

        // GET: BorrowBook
        public ActionResult Index()
        {
            CheckExpire();
            var borrowBooks = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Book).Include(b => b.Category).Include(b => b.Borrower);
            return View(borrowBooks.ToList());
        }

        // GET: BorrowBook/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return HttpNotFound();
            }
            return View(borrowBook);
        }

        // GET: BorrowBook/Create
        public ActionResult Create()
        {
            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name");
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_name");
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            ViewBag.borrower_id = new SelectList(db.Borrowers, "borrower_id", "borrower_name");
            return View();
        }

        // POST: BorrowBook/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BorrowBook borrowBook)
        {
            if (ModelState.IsValid)
            {
                //borrowBook.borrow_date = DateTime.Today;

                db.BorrowBooks.Add(borrowBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name", borrowBook.admin_id);
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_name", borrowBook.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", borrowBook.category_id);
            ViewBag.borrower_id = new SelectList(db.Borrowers, "borrower_id", "borrower_name", borrowBook.borrower_id);
            return View(borrowBook);
        }

        // GET: BorrowBook/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name", borrowBook.admin_id);
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_name", borrowBook.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", borrowBook.category_id);
            ViewBag.borrower_id = new SelectList(db.Borrowers, "borrower_id", "borrower_name", borrowBook.borrower_id);
            return View(borrowBook);
        }

        // POST: BorrowBook/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BorrowBook borrowBook)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(borrowBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name", borrowBook.admin_id);
            ViewBag.book_id = new SelectList(db.Books, "book_id", "book_name", borrowBook.book_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", borrowBook.category_id);
            ViewBag.borrower_id = new SelectList(db.Borrowers, "borrower_id", "borrower_name", borrowBook.borrower_id);
            return View(borrowBook);
        }

        // GET: BorrowBook/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return HttpNotFound();
            }
            return View(borrowBook);
        }

        // POST: BorrowBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            db.BorrowBooks.Remove(borrowBook);
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
