using Kipodeal.Helper.Cache;
using Michal.Project.Contract;
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
        static List<KeyValuePair<int, string>> _backOrderItems = null;
        static List<KeyValuePair<int, string>> _getDirection = null;
        static StreetsGeoLocation _locationDes = null;
        static Guid _orgId = Guid.Empty;
        static Organization _organization = null;

        public MemeryCacheDataService()
        {

        }

        public Guid GetOrg(ApplicationDbContext context)
        {
            if (_orgId == Guid.Empty)
            {
                lock (lockObj)
                {
                    if (_orgId == Guid.Empty)
                    {
                        var orgconfig = System.Configuration.ConfigurationManager.AppSettings["org"].ToString();
                        var org = context.Organization.Where(o => o.Name == orgconfig).FirstOrDefault();
                        if (org != null)
                        {
                            _orgId = org.OrgId;
                        }

                    }
                }
            }
            return _orgId;
        }

        public Organization GetOrgEntity(ApplicationDbContext context)
        {
            if (_organization == null)
            {
                lock (lockObj)
                {
                    if (_organization == null)
                    {
                        var orgconfig = System.Configuration.ConfigurationManager.AppSettings["org"].ToString();
                        var org = context.Organization.Where(o => o.Name == orgconfig).FirstOrDefault();
                        if (org != null)
                        {
                            _organization = org;
                        }

                    }
                }
            }
            return _organization;
        }

        public List<KeyValuePair<int, string>> GetDirection()
        {
            if (_getDirection == null)
            {
                lock (lockObj)
                {
                    if (_getDirection == null)
                    {
                        _getDirection = new List<KeyValuePair<int, string>>();
                        _getDirection.Add(new KeyValuePair<int, string>(0, "משולח ההזמנה אל היעד"));
                        _getDirection.Add(new KeyValuePair<int, string>(1, "מהיעד לשולח הזמנה"));
                      
                    }
                }
            }
            return _getDirection;
        }
       
        public List<KeyValuePair<int,string>> GetBackOrder()
        {
            if (_backOrderItems == null)
            {
                lock (lockObj)
                {
                    if (_backOrderItems == null)
                    {
                        _backOrderItems = new List<KeyValuePair<int, string>>();
                        _backOrderItems.Add(new KeyValuePair<int, string>(0, "ללא חזרה"));
                        _backOrderItems.Add(new KeyValuePair<int, string>(1, "ללא חזרה - חתימה במכשיר"));
                        _backOrderItems.Add(new KeyValuePair<int, string>(2, "חזרה- עם חתימה"));
                    }
                }
            }
            return _backOrderItems;
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

        public List<PriceList> GetPriceList(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<PriceList> lists = null;
            cacheMemoryProvider.Get("GetPriceList", out lists);
            if (lists == null)
            {
                lists = (from pr in context.PriceList
                         where pr.IsActive == true && !pr.EndDate.HasValue
                         select pr
                              ).ToList();
                cacheMemoryProvider.Set("GetPriceList", lists);
            }
            return lists;

        }

        public List<Discount> GetDiscount(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Discount> lists = null;
            cacheMemoryProvider.Get("GetDiscount", out lists);
            if (lists == null)
            {
                lists = (from pr in context.Discount
                         where pr.IsActive == true && !pr.EndDate.HasValue
                         select pr
                              ).ToList();
                cacheMemoryProvider.Set("GetDiscount", lists);
            }
            return lists;

        }

        public List<Product> GetProducts(ApplicationDbContext context, Guid orgId)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Product> lists = null;
            cacheMemoryProvider.Get("GetProducts", out lists);
            if (lists == null)
            {
                lists = context.Product.Where(s => s.Organizations.Any(e =>  e.OrgId == orgId)).ToList();
                cacheMemoryProvider.Set("GetProducts", lists);
            }
            return lists;
        }

        public List<ProductSystem> GetProductsSystem(ApplicationDbContext context)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<ProductSystem> lists = null;
            cacheMemoryProvider.Get("GetProductsSystem", out lists);
            if (lists == null)
            {
                lists = context.ProductSystem.Where(s => s.IsActive==true).ToList();
                cacheMemoryProvider.Set("GetProductsSystem", lists);
            }
            return lists;
        }

        public List<ShippingCompany> GetShippingCompaniesByOrgId(ApplicationDbContext context,Guid orgId)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<ShippingCompany> lists = null;
            cacheMemoryProvider.Get("GetShippingCompaniesByOrgId", out lists);
            if (lists == null)
            {
                lists = context.ShippingCompany.Where(s => s.IsActive == true && s.Organizations.Any(o=>o.OrgId==orgId)).ToList();
                cacheMemoryProvider.Set("GetShippingCompaniesByOrgId", lists);
            }
            return lists;
        }

        public List<Sla> GetSlaByOrgId(ApplicationDbContext context, Guid orgId)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<Sla> lists = null;
            cacheMemoryProvider.Get("GetSlaByOrgId", out lists);
            if (lists == null)
            {
                lists = context.Sla.Where(s => s.IsActive == true && s.Organizations_OrgId.HasValue && s.Organizations_OrgId.Value==orgId).ToList();
                cacheMemoryProvider.Set("GetSlaByOrgId", lists);
            }
            return lists;
        }

        //public List<ApplicationUser> GetManagersOfCompanies(ApplicationDbContext context)
        //{
        //    CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
        //    List<ApplicationUser> lists = null;
        //    cacheMemoryProvider.Get("GetManagersOfCompany", out lists);
        //    if (lists == null)
        //    {
        //        lists = context.Users.Where(s => s.IsActive == true && s.Roles.Any(r=>r.Role.Name== HelperAutorize.RunnerManager || r.Role.Name==HelperAutorize.RoleAdmin)).ToList();
        //        cacheMemoryProvider.Set("GetManagersOfCompany", lists);
        //    }
        //    return lists;
        //}

        public List<IBussinessClosure> GetBussinessClosureByCompanyShip(ApplicationDbContext context, Guid companyShipId)
        {
            var key = "GetBussinessClosureByCompanyShip_" + companyShipId.ToString();
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<IBussinessClosure> lists = null;
            cacheMemoryProvider.Get(key, out lists);
            if (lists == null)
            {
                lists = context.BussinessClosure.Where(s => s.IsActive == true && s.ShippingCompany == companyShipId).ToList<IBussinessClosure>();
                cacheMemoryProvider.Set(key, lists);
            }
            return lists;
        }
    }
}