using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BMS.Env;
using BMS.Models;
using EntityState = System.Data.Entity.EntityState;

namespace BMS.Controllers
{
    public class BorrowBookController : Controller
    {
        private BMSDATAEntities db = new BMSDATAEntities();

        public bool CheckLogin()
        {

            if ((string)Session["Username"] == null)
            {
                    return false;
            }

            string authenToken = CacheData.GetDataFromCache(Session["Username"].ToString());

            if (authenToken == null)
            {
                return false;
            }

            if (Secure.Decrypt(authenToken) != Secure.Decrypt(Secure.Decrypt(Session["Token"].ToString())))
            {
                return false;
            }
            return true;
        }

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
        
        public ActionResult Index(string selectSort,string search)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }
            var borrowBooksSearch = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Book).Include(b => b.Category).Include(b => b.Borrower);
            CheckExpire();

            if (selectSort == "Earliest")
            {
                var borrowBooksSortNewest = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Book).Include(b => b.Category).Include(b => b.Borrower).OrderBy(b => b.borrow_date);
                return View(borrowBooksSortNewest.ToList());
            }

            if (selectSort == "Latest")
            {
                var borrowBooksSortLatest = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Book).Include(b => b.Category).Include(b => b.Borrower).OrderByDescending(b => b.borrow_date);
                return View(borrowBooksSortLatest.ToList());
            }

            var borrowBooksSearchResult = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Category).Include(b => b.Category).Include(b => b.Borrower).Where(b => b.Book.book_name.Contains(search) || b.Borrower.borrower_name.Contains(search) || b.Category.category_name.Contains(search) ||b.Admin.admin_name.Contains(search));
            if (borrowBooksSearchResult != null && search!=null)
            {
                return View(borrowBooksSearchResult);

            }
            return View(borrowBooksSearch);

            //var borrowBooksSort = db.BorrowBooks.Include(b => b.Admin).Include(b => b.Book).Include(b => b.Category).Include(b => b.Borrower).OrderByDescending(b=>b.borrow_date);
            //return View(borrowBooksSort.ToList());
        }

        // GET: BorrowBook/Details/5
        public ActionResult Details(int? id)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 500, message = "Internal Server Error" });

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

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (borrowBook == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 500, message = "Internal Server Error" });

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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }


            if (borrowBook == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

            BorrowBook borrowBook = db.BorrowBooks.Find(id);
            if (borrowBook == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 500, message = "Internal Server Error" });

            }
            return View(borrowBook);
        }

        // POST: BorrowBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

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
