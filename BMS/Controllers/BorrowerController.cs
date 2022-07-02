using System;
using System.Collections.Generic;
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
    public class BorrowerController : Controller
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

        // GET: Borrower
        public ActionResult Index(string search)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }
            if(search == null)
            {
                return View(db.Borrowers.ToList());
            }
            var borrowers = db.Borrowers.Where(b => b.borrower_name.Contains(search) || b.borrower_email.Contains(search)|| b.borrower_phone== search || b.borrower_address.Contains(search);
            if(borrowers != null)
            {
                return View("Index", borrowers);
            }
            return View(db.Borrowers.ToList());
        }

        // GET: Borrower/Details/5
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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }


            if (borrower == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }


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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (borrower == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }

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
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (id == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
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
