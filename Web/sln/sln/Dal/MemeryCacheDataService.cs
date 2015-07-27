﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Dal
{
    public class ViewItem
    {
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }

        public bool IsDefaultAdmin { get; set; }
        public bool IsDefaultOrgManager { get; set; }
        public bool IsDefaultAccept { get; set; }
        public bool IsDefaultUser { get; set; }
        public bool IsDefaultRunner{ get; set; }

        public bool IsVisbleForAdmin { get; set; }
        public bool IsVisbleForOrgManager { get; set; }
        public bool IsVisbleForAccept { get; set; }
        public bool IsVisbleForUser { get; set; }
        public bool IsVisbleForRunner { get; set; }

    }
    public class MemeryCacheDataService
    {
        static object lockObj = new object();
        static List<ViewItem> _viewItems;
       // static Dictionary<int, string> _views;
        public MemeryCacheDataService(ApplicationDbContext context)
        {

        }

        public MemeryCacheDataService()
        {

        }
        public List<ViewItem> GetView()
        {
            if (_viewItems == null)
            {
                lock (lockObj)
                {
                    if (_viewItems == null)
                    {
                        _viewItems = new List<ViewItem>();
                        _viewItems.Add(new ViewItem { StatusId = 1, StatusDesc = "משלוחים טויטה - היום",IsDefaultUser=true});
                        _viewItems.Add(new ViewItem { StatusId = 2, StatusDesc = "משלוחים שהוזמנו" });
                    }
                }
            }
            return _viewItems;
        }
        //public Dictionary<int, string> GetView()
        //{
        //    if (_views == null)
        //    {
        //        lock (lockObj)
        //        {
        //            if (_views == null)
        //            {
        //                _views = new Dictionary<int, string>();
        //                _views.Add(1, "תצוגה משלוחים חדשים שלי- טויטה ");
        //                _views.Add(2, "משלוחים ממתין לאישור מנהל");
        //                _views.Add(3, "משלוחים ממתין לאישור רן שליחיות");
        //            }
        //        }
        //    }
        //    return _views;
        //}
    }
}