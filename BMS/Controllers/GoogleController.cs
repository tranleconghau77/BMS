using BMS.env;
using BMS.Env;
using BMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMS.Controllers
{
    public class GoogleController : Controller
    {
        // GET: Google
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void GoogleLogin(string email, string name, string gender, string lastname, string location,string img)
        {
            //Write your code here to access these paramerters
            Admin admin = new Admin();
            admin.admin_username = email;
            admin.admin_name = name;
            admin.admin_phone = "";
            admin.admin_image = img;

            Session["Username"] = email;
            Session["Token"] = Secure.Encrypt(Secure.Encrypt(Variable.TokenCode));



        }
    }
}