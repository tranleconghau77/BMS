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
using BMS.Models;
using EntityState = System.Data.Entity.EntityState;

namespace BMS.Controllers
{
    public class BookController : Controller
    {
        private BMSDATAEntities db = new BMSDATAEntities();

        // GET: Book
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
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
            return View(books.ToList());
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Search")]
        [ValidateAntiForgeryToken]
        public ActionResult Search(String search)
        {
            if(search == null)
            {
                return RedirectToAction("Index");
            }
            var books = db.Books.Include(b => b.Author).Include(b => b.Category).Where(b=>b.book_name.Contains(search) || b.Author.author_name.Contains(search) || b.Category.category_name.Contains(search));
            return View("Index",books);
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
