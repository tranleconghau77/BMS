using BMS.Env;
using BMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BMS.Controllers
{
    public class StatisticController : Controller
    {
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

        private BMSDATAEntities db = new BMSDATAEntities();
        // GET: Statistic

        List<Int32> Time = new List<Int32>();
        List<Int32> ResultCountPerMonth = new List<Int32>();
        List<Int32> StatusLateCountPerMonth = new List<Int32>();
        public void InitData()
        {
            Time.Add(1);
            Time.Add(2);
            Time.Add(3);
            Time.Add(4);
            Time.Add(5);
            Time.Add(6);
            Time.Add(7);
            Time.Add(8);
            Time.Add(9);
            Time.Add(10);
            Time.Add(11);
            Time.Add(12);

            using (var ctx = new BMSDATAEntities())
            {
                foreach (Int32 id in Time)
                {
                    var x = db.BorrowBooks.Where(y => y.borrow_date.Value.Month == id && y.borrow_date.Value.Year == 2022).Count();
                    ResultCountPerMonth.Add((x));

                    var z = db.BorrowBooks.Where(y => y.borrow_date.Value.Month == id && y.status == "Late").Count();
                    StatusLateCountPerMonth.Add((z));

                }
            }
        }
        public ActionResult Index()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");

            }

            InitData();

            return View();
        }

        public ActionResult BorrowDataStatistic()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            InitData();

            var output= JsonConvert.SerializeObject(ResultCountPerMonth);

            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult StatusDataStatistic()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("LoginPage", "Login");
            }

            InitData();

            var output = JsonConvert.SerializeObject(StatusLateCountPerMonth);

            return Json(output, JsonRequestBehavior.AllowGet);
        }
    }
}