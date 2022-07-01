
using BMS.Env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMS.Controllers
{


    public class HomeController : Controller
    {
        public bool CheckLogin()
        {
            
            string authenToken = CacheData.GetDataFromCache("AuthenToken");
            if (Secure.Decrypt(authenToken) != Secure.Decrypt(Secure.Decrypt(Session["Token"].ToString()))){
                return false;
            }
            return true;
        }
        public ActionResult Index()
        {
            //    if (!CheckLogin())
            //    {
            //    return RedirectToAction("LoginPage","Login");
            //    }
              return View();
            
        }

        public ActionResult About()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}