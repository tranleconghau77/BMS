using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BMS;
using BMS.Env;
using BMS.Models;
using EntityState = System.Data.EntityState;

namespace BMS.Controllers
{
    public class AdminController : Controller
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


        // GET: Admin/Details/5
        public ActionResult Details()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }
            
            string username = Session["Username"].ToString();
            var result = db.Admins.Where(y => y.admin_username == username).FirstOrDefault();
            
            return View(result);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string name)
        {
            if(name == null){
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message="Bad Request" });
            }
           
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            string user_name = name + "@gmail.com";
            if (user_name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Where(y=>y.admin_username== user_name).FirstOrDefault();
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Admin admin)
        {
            if (admin == null)
            {
                return RedirectToAction("Error", "ErrorPage", new { statusCode = 400, message = "Bad Request" });
            }


            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            if (ModelState.IsValid)
            {
                Session["Username"]=admin.admin_username;
                if (admin.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(admin.ImageUpload.FileName);
                    string extension = Path.GetExtension(admin.ImageUpload.FileName);
                    filename = filename + extension;
                    admin.admin_image = "~/Images/Admin/" + filename;
                    admin.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/Admin/"), filename));
                }
                db.Entry(admin).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();
                return View("Details",admin);
            }
            return View("Index");
        }

        // GET: Admin/Delete/5




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
