using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Dal
{
    public class MemeryCacheDataService
    {
        static object lockObj = new object();
        static Dictionary<int, string> _views;
        public MemeryCacheDataService(ApplicationDbContext context)
        {

        }

        public Dictionary<int, string> GetView()
        {
            if (_views == null)
            {
                lock (lockObj)
                {
                    if (_views == null)
                    {
                        _views = new Dictionary<int, string>();
                        _views.Add(1, "תצוגה משלוחים חדשים שלי- טויטה ");
                        _views.Add(2, "משלוחים ממתין לאישור מנהל");
                        _views.Add(3, "משלוחים ממתין לאישור רן שליחיות");


                    }
                }
            }
            return _views;
        }
    }
}