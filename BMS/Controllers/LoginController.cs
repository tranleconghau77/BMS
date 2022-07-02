using BMS.env;
using BMS.Env;
using BMS.Models;
using BMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BMS.Controllers
{
    public class LoginController : Controller
    {
      
        private BMSDATAEntities db = new BMSDATAEntities();
        
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(Login person)
        {
            ViewBag.AccountIsWrong = false;
            var password =Secure.Encrypt(person.Password);
            var checkAccount=db.Admins.Where(p=>p.admin_username==person.Username && p.admin_password==password).FirstOrDefault();
            if (checkAccount==null)
            {
                ViewBag.AccountIsWrong = true;  
                return View("LoginPage");
            }
            Session["UserName"] = checkAccount.admin_username;
            Session["Token"] = Secure.Encrypt(Secure.Encrypt(Variable.TokenCode));
            Session["UserImage"] = checkAccount.admin_image;
            Session["TypeAccount"] = "0";
            CacheData.SetDataFromCache(checkAccount.admin_username,Secure.Encrypt(Variable.TokenCode));
            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        public void GoogleLogin(string email, string name,string img)
        {
            //Write your code here to access these paramerters


            string pathStore = @"D:\UEF\WEB\BMS\BMS\Images\Admin\";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(img), pathStore + name+".png");
            }

            string pathDataBase = @"~/Images/Admin/"+name+".png";

            var result = db.Admins.Where(y => y.admin_username == email).ToList();
            if (result.Count==0)
            {
                Admin admin = new Admin();
                admin.admin_username = email;
                admin.admin_name = name;
                admin.admin_image = pathDataBase;
                db.Admins.Add(admin);
                db.SaveChanges();
            }

            Session["Username"] = email;
            Session["Token"] = Secure.Encrypt(Secure.Encrypt(Variable.TokenCode));
            Session["UserImage"] = pathDataBase;
            Session["TypeAccount"] = "1";

            Session["Token"] = Secure.Encrypt(Secure.Encrypt(Variable.TokenCode));
            CacheData.SetDataFromCache(email, Secure.Encrypt(Variable.TokenCode));
        }
        public ActionResult SignOut()
        {

            string type = (string) Session["TypeAccount"];
            if (type=="1")
            {
                int username = Int32.Parse((string)Session["TypeAccount"]);
                if (username == 1)
                {
                    Session.Clear();
                    return RedirectToAction("LoginPage"); ;
                }
            }
            Session.Clear();
            return RedirectToAction("LoginPage");
        }

    }
}
