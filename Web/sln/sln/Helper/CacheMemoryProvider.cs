
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kipodeal.Helper.Cache
{
    public class CacheMemoryProvider 
    {
        int DefaultCacheHours=24;
        public CacheMemoryProvider()
        {

        }

        const string cache = "~/cache.txt";

        public void Refresh(string s)
        {
            string cachedFilePath = HttpContext.Current.Server.MapPath(cache);
            using (var data = System.IO.File.AppendText(cachedFilePath))
            {
                data.Write(s);
            }

        }

        public bool Get<T>(string key, out T value)
        {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(key))
               value = (T)cache.Get(key);
            
            else
                value = default(T);
            return cache.Contains(key);

        }


        public void Set<T>(string key, T value, int? duration = null)
        {
            ObjectCache cache = MemoryCache.Default;
            if (!cache.Contains(key))
            {
                cache.Add(key, value, GetCacheItemPolicy(duration));
            }
            else
            {
                cache[key] = value;
            }
        }

        public void Clear(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(key);
        }

        CacheItemPolicy GetCacheItemPolicy(int? duration = null)
        {
            var durationHours = duration.HasValue ? duration.Value :DefaultCacheHours;
            
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(durationHours);

            List<string> filePaths = new List<string>();
            string cachedFilePath = HttpContext.Current.Server.MapPath(cache); //  rootPathDependency + "/" + ConstantVariables.DefaultCahceFile;
            var uCache = new Uri(cachedFilePath);

            filePaths.Add(uCache.LocalPath);
            
            policy.ChangeMonitors.Add(new
                HostFileChangeMonitor(filePaths));


            return policy;
        }

    }
}
