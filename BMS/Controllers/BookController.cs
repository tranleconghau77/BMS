using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BMS.Env;
using BMS.Models;
using EntityState = System.Data.Entity.EntityState;

namespace BMS.Controllers
{
    public class BookController : Controller
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

        // GET: Book
        public ActionResult Index(string sortOrder,string search)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "name";
            ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author" : "author";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category" : "category";
            var books = db.Books.Include(b => b.Author).Include(b => b.Category);
            switch (sortOrder)
            {
                case "name":
                    books = books.OrderByDescending(s => s.book_name);
                    break;
                case "author":
                    books = books.OrderByDescending(s => s.Author.author_name);
                    break;
                case "category":
                    books = books.OrderByDescending(s => s.Category.category_name);
                    break;
                default:
                    books = books.OrderBy(s => s.book_name);
                    break;
            }

            if (search == null)
            {
                return View(books);
            }
             books = db.Books.Include(b => b.Author).Include(b => b.Category).Where(b => b.book_name.Contains(search) || b.Author.author_name.Contains(search) || b.Category.category_name.Contains(search));
            if (books == null)
            {
                return View(books);

            }
            return View(books);
            
        }

        // GET: Book/Details/5
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

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            Book book = new Book();
            ViewBag.book_author = new SelectList(db.Authors, "author_id", "author_name");
            ViewBag.book_category = new SelectList(db.Categories, "category_id", "category_name");
            return View(book);
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Book book)
        {

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (book == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }


            if (ModelState.IsValid)
            {
                if (book.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(book.ImageUpload.FileName);
                    string extension = Path.GetExtension(book.ImageUpload.FileName);
                    filename = filename + extension;
                    book.book_image = "~/Images/Book/" + filename;
                    book.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/Book/"), filename));
                }
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.book_author = new SelectList(db.Authors, "author_id", "author_name", book.book_author);
            ViewBag.book_category = new SelectList(db.Categories, "category_id", "category_name", book.book_category);
            return View(book);
        }

        // GET: Book/Edit/5
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

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.book_author = new SelectList(db.Authors, "author_id", "author_name", book.book_author);
            ViewBag.book_category = new SelectList(db.Categories, "category_id", "category_name", book.book_category);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Book book)
        {

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (book == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

            if (ModelState.IsValid)
            {
                if (book.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(book.ImageUpload.FileName);
                    string extension = Path.GetExtension(book.ImageUpload.FileName);
                    filename = filename + extension;
                    book.book_image = "~/Images/Book/" + filename;
                    book.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/Book/"), filename));
                }
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.book_author = new SelectList(db.Authors, "author_id", "author_name", book.book_author);
            ViewBag.book_category = new SelectList(db.Categories, "category_id", "category_name", book.book_category);
            return View(book);
        }

        // GET: Book/Delete/5
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

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 500, message = "Internal Server Error" }); ;
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
