using Kipodeal.Helper.Cache;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Dal
{


    public class MemeryCacheDataService
    {
        static object lockObj = new object();
        static List<ViewItem> _viewItems;

        public MemeryCacheDataService()
        {

        }

        public List<City> GetCities(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<City> cities = null;
            cacheMemoryProvider.Get("City", out cities);
            if (cities == null)
            {
                cities = context.City.ToList();
                cacheMemoryProvider.Set("City", cities);
            }
            return cities;
        }

        public List<Organization> GetOrgs(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Organization> lists = null;
            cacheMemoryProvider.Get("Org", out lists);
            if (lists == null)
            {
                lists = context.Organization.ToList();
                cacheMemoryProvider.Set("Org", lists);
            }
            return lists;
        }

        public List<Distance> GetDistancesPerOrg(ApplicationDbContext context, Guid orgId)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Distance> lists = null;
            cacheMemoryProvider.Get("DistancesPerOrg", out lists);
            if (lists == null)
            {
                lists = context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToList();

                cacheMemoryProvider.Set("DistancesPerOrg", lists);
            }
            return lists;
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
                        var viewItem = new ViewItem { StatusId = TimeStatus.New, StatusDesc = "משלוחים טויטה - היום" };
                        viewItem.FieldShowMy = "OwnerId";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.ApporvallRequest, StatusDesc = "משלוחים שהוזמנו" };
                        viewItem.FieldShowMy = "ApprovalRequest";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.Confirm, StatusDesc = "משלוחים שאושרו ע''י חברת השליחים" };
                        viewItem.FieldShowMy = "ApprovalShip";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.CancelByAdmin, StatusDesc = "משלוחים שבוטלו ע''י חברת השליחים" };
                        viewItem.FieldShowMy = "CancelByAdmin";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.AcceptByRunner, StatusDesc = "משלוחים שנמצאים אצל השליח" };
                        viewItem.FieldShowMy = "BroughtShipmentCustomer";
                        _viewItems.Add(viewItem);
                    }
                }
            }
            return _viewItems;
        }

        public List<Runner> GetRunners(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Runner> lists = null;
            cacheMemoryProvider.Get("Runner", out lists);
            if (lists == null)
            {
                lists = (from r in context.Users
                         where r.IsActive == true && r.Roles.Any(ro => ro.Role != null &&
                             (ro.Role.Name == Helper.HelperAutorize.RoleRunner || ro.Role.Name == Helper.HelperAutorize.RoleAdmin))
                         select new Runner
                         {
                             Id = r.Id,
                             FirstName = r.FirstName,
                             Lastname = r.LastName
                         }
                              ).ToList();
                cacheMemoryProvider.Set("Runner", lists);
            }
            return lists;

        }

        public List<ShipType> GetShipType(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<ShipType> lists = null;
            cacheMemoryProvider.Get("ShipType", out lists);
            if (lists == null)
            {
                lists = context.ShipType.ToList();
                cacheMemoryProvider.Set("ShipType", lists);
            }
            return lists;

        }
    }
}