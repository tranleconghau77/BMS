using BMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace BMS.Env
{
    public class CacheData
    {

        public static string GetDataFromCache(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheKey))
                return cache.Get(cacheKey).ToString();
            else
            {

                return null;
            }
        }

        public static void SetDataFromCache(string CacheKey, string data)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(24.0);

            if (!cache.Contains(CacheKey))
                cache.Add(CacheKey, data, cacheItemPolicy);

            
        }

    }
}