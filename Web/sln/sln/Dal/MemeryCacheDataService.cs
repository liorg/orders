using Kipodeal.Helper.Cache;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Michal.Project.Dal
{


    public class MemeryCacheDataService
    {
        static object lockObj = new object();
        static List<ViewItem> _viewItems;
        static StreetsGeoLocation _locationDes = null;
        public MemeryCacheDataService()
        {

        }

        public StreetsGeoLocation GetStreetsGeoLocation()
        {
            string path = System.Web.HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/") + "rechov.json";
            if (_locationDes == null)
            {
                lock (lockObj)
                {
                    if (_locationDes == null)
                    {
                        using (StreamReader file = File.OpenText(path))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            _locationDes = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                        }
                    }
                }
            }
            return _locationDes;
        }

        public IEnumerable<KeyValuePairUI> GetStreetByCityCode(string cityCode,string term,int maxTake)
        {
            StreetsGeoLocation locationDes = GetStreetsGeoLocation();
            var query = (from cm in locationDes.StreetsItems
                        where cm.CodeCity == cityCode && cm.Addr.Contains(term)
                       select
                        new KeyValuePairUI(cm.CodeAddr, cm.Addr)).Take(maxTake);
            return query.ToList();
        }

        public IEnumerable<KeyValuePairUI> GetCitiesByName(string term)
        {
            var cities = GetCities();
            return cities.Where(c => c.Value.Contains(term)).ToList();
        }
        public IEnumerable<KeyValuePairUI> GetCities()
        {
             StreetsGeoLocation locationDes = GetStreetsGeoLocation();
            var query = from cm in locationDes.StreetsItems
                        group cm by new { cm.CodeCity, cm.City } into cms
                        select
                        new KeyValuePairUI(cms.Key.CodeCity, cms.Key.City);
            return query.ToList();
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
            cacheMemoryProvider.Get("DistancesPerOrg_" + orgId.ToString(), out lists);
            if (lists == null)
            {
                lists = context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToList();

                cacheMemoryProvider.Set("DistancesPerOrg_" + orgId.ToString(), lists);
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
                        var viewItem = new ViewItem { StatusId = TimeStatus.New, StatusDesc = "משלוחים טויטה " };
                        viewItem.FieldShowMy = "OwnerId";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.ApporvallRequest, StatusDesc = "משלוחים שהוזמנו " };
                        viewItem.FieldShowMy = "ApprovalRequest";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.Confirm, StatusDesc = "משלוחים שאושרו והועברו לשליח  " };
                        viewItem.FieldShowMy = "GrantRunner";// "ApprovalShip";
                        _viewItems.Add(viewItem);


                        viewItem = new ViewItem { StatusId = TimeStatus.CancelByAdmin, StatusDesc = " משלוחים שבוטלו ע''י חברת השליחים " };
                        viewItem.FieldShowMy = "CancelByAdmin";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.AcceptByRunner, StatusDesc = "משלוחים שנמצאים אצל השליח " };
                        viewItem.FieldShowMy = "BroughtShipmentCustomer";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.Arrived, StatusDesc = "משלוחים בזמן המתנה " };
                        viewItem.FieldShowMy = "ArrivedShippingSender";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.NoAcceptByClient, StatusDesc = "משלוחים שלא הגיעו ללקוח " };
                        viewItem.FieldShowMy = "NoBroughtShipmentCustomer";
                        _viewItems.Add(viewItem);

                        viewItem = new ViewItem { StatusId = TimeStatus.AcceptByClient, StatusDesc = "משלוחים שהגיעו ללקוח " };
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