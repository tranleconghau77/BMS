using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMS.Controllers
{
    public class ErrorPageController : Controller
    {
        // GET: Error
        public ActionResult Error(int? statusCode, string message)
        {
            if (statusCode==null || message == null)
            {
                return View("Error");
            }
            Response.StatusCode = statusCode.Value ;
            ViewBag.StatusCode = statusCode.Value + "\t"+ message;
            return View();
        }
    }
}