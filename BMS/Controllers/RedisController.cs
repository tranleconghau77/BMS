using BMS.Models;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BMS.Controllers
{
    public class RedisController : Controller
    {

        private static BMSDATAEntities db = new BMSDATAEntities();

        // GET: Redis
            private const string CacheKey = "availableStocks";
        public ActionResult Index()
        {
            StockItems PS = new StockItems();
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);
            List<Book> databasestateList = (from book in db.Books
                                            select book).ToList();

            IEnumerable availableStocks = databasestateList;

            List<Book>Pizzas = (List<Book>)PS.GetAvailableStocks(CacheKey, availableStocks, cacheItemPolicy);
            ViewBag.Pizzas = Pizzas;
            return View();

           
        }
        public class StockItems
        {

            public IEnumerable GetAvailableStocks(string CacheKey, IEnumerable availableStocks, CacheItemPolicy cacheItemPolicy)
            {
                ObjectCache cache = MemoryCache.Default;

                if (cache.Contains(CacheKey))
                    return (IEnumerable)cache.Get(CacheKey);
                else
                {
                     

                    // Store data in the cache    
                    
                    cache.Add(CacheKey, availableStocks, cacheItemPolicy);

                    return availableStocks;
                }
            }
            //public IEnumerable GetDefaultStocks()
            //{
            //   List<Book>databasestateList = (from book in db.Books
            //                                 select book).ToList();
            //    return databasestateList;
            //}
        }
    }
}