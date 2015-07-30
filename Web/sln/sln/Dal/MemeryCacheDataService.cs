﻿using sln.Helper;
using sln.Models;
using System;
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
                        _viewItems.Add(new ViewItem { StatusId = TimeStatus.New, StatusDesc = "משלוחים טויטה - היום",IsDefaultUser=true});
                        _viewItems.Add(new ViewItem { StatusId = TimeStatus.ApporvallRequest, StatusDesc = "משלוחים שהוזמנו",IsDefaultUser=true,IsVisbleForAdmin=true,IsDefaultAdmin=true });
                        _viewItems.Add(new ViewItem { StatusId = TimeStatus.Confirm, StatusDesc = "משלוחים שאושרו ע''י חברת השליחים" ,IsVisbleForAdmin=true});
                        _viewItems.Add(new ViewItem { StatusId = TimeStatus.CancelByAdmin, StatusDesc = "משלוחים שבוטלו ע''י חברת השליחים" });
                        _viewItems.Add(new ViewItem { StatusId = TimeStatus.AcceptByRunner, StatusDesc = "משלוחים שנמצאים אצל השליח" });
                    }
                }
            }
            return _viewItems;
        }

        public List<Runner> GetRunners(ApplicationDbContext context)
        {
            return (from r in context.Users
                    where r.IsActive == true && r.Roles.Any(ro => ro.Role != null && ro.Role.Name == Helper.HelperAutorize.RoleRunner)
                    select new Runner
                    {
                        Id = r.Id,
                        FirstName = r.FirstName,
                        Lastname = r.LastName
                    }
                            ).ToList();
        }
    }
}