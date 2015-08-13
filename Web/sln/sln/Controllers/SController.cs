using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Michal.Project.Bll;
using System.Linq.Expressions;
using Michal.Project.Models.View;
namespace Michal.Project.Controllers
{
    [Authorize]
    public class SController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Index(int? viewType, bool? viewAll, int? currentPage, string nextDay, string prevDay)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                int order = viewType.HasValue ? viewType.Value : user.DefaultView;
                var view = cache.GetView().Where(g => g.StatusId == order).FirstOrDefault();
                if (view == null)
                {
                    view = new ViewItem { StatusId = TimeStatus.New, StatusDesc = "משלוחים טויטה - היום" };
                    view.FieldShowMy = "OwnerId";
                }

                if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
                    orgId = Guid.Empty; //user.OrgId;
                var showAll = viewAll == null ? user.ShowAll : viewAll.Value;
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1).Date;
                var to = DateTime.Today.AddDays(1).Date;

                if (!String.IsNullOrEmpty(nextDay))
                    to = DateTime.ParseExact(nextDay, "yyyy-MM-dd", null);

                if (!String.IsNullOrEmpty(prevDay))
                    from = DateTime.ParseExact(prevDay, "yyyy-MM-dd", null);


                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && (s.CreatedOn > from && s.CreatedOn <= to) && s.Organization_OrgId.HasValue && (s.Organization_OrgId.Value == orgId || orgId == Guid.Empty)).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

                if (!showAll)
                    shippingsQuery = shippingsQuery.Where(view.GetMyRecords(user)).AsQueryable();

                int page = currentPage.HasValue ? currentPage.Value : 1;
                var total = await shippingsQuery.CountAsync();
                var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

                shippings = await shippingsQuery.OrderByDescending(ord => ord.ModifiedOn).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();
                var shippingsItems = new List<ShippingVm>();
                foreach (var ship in shippings)
                {

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
                    u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
                    u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
                    u.NumFrom = ship.AddressNumFrom;
                    u.NumTo = ship.AddressNumTo;
                    u.SreetFrom = ship.AddressFrom;
                    u.SreetTo = ship.AddressTo;
                    u.TelTarget = ship.TelTarget;
                    u.NameTarget = ship.NameTarget;

                    shippingsItems.Add(u);
                }

                bool isToday = to.Date == DateTime.Now.AddDays(1).Date;

                ViewBag.BShowAll = showAll;
                ViewBag.ShowAll = showAll.ToString();
                ViewBag.Total = total;
                ViewBag.CurrentPage = page;
                ViewBag.MoreRecord = hasMoreRecord;
                ViewBag.Selected = view.StatusDesc;
                ViewBag.StatusId = view.StatusId;
                ViewBag.FromDay = from.ToString("yyyy-MM-dd");
                ViewBag.ToDay = to.ToString("yyyy-MM-dd");
                ViewBag.IsToday = isToday;
                ViewBag.Title = view.StatusDesc + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");

                SpecialView specialView = new SpecialView();
                specialView.Items = shippingsItems.AsEnumerable();
                

                specialView.BShowAll = showAll;
                specialView.ShowAll = showAll.ToString();

                specialView.Total = total;
                specialView.CurrentPage = page;
                specialView.MoreRecord = hasMoreRecord;
                specialView.Title = view.StatusDesc + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");
                
                specialView.FromDay = from.ToString("yyyy-MM-dd");
                specialView.ToDay = to.ToString("yyyy-MM-dd");
                specialView.IsToday = to.Date == DateTime.Now.AddDays(1).Date;

                return View(specialView);
            }
        }

        public async Task<ActionResult> IndexOld(int? viewType, bool? viewAll, int? currentPage, string nextDay, string prevDay)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                int order = viewType.HasValue ? viewType.Value : user.DefaultView;
                var view = cache.GetView().Where(g => g.StatusId == order).FirstOrDefault();
                if (view == null)
                {
                    view = new ViewItem { StatusId = TimeStatus.New, StatusDesc = "משלוחים טויטה - היום" };
                    view.FieldShowMy = "OwnerId";
                }

                if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
                    orgId = Guid.Empty; //user.OrgId;
                var showAll = viewAll == null ? user.ShowAll : viewAll.Value;
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1).Date;
                var to = DateTime.Today.AddDays(1).Date;

                if (!String.IsNullOrEmpty(nextDay))
                    to = DateTime.ParseExact(nextDay, "yyyy-MM-dd", null);

                if (!String.IsNullOrEmpty(prevDay))
                    from = DateTime.ParseExact(prevDay, "yyyy-MM-dd", null);


                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && (s.CreatedOn > from && s.CreatedOn <= to) && s.Organization_OrgId.HasValue && (s.Organization_OrgId.Value == orgId || orgId == Guid.Empty)).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

                if (!showAll)
                    shippingsQuery = shippingsQuery.Where(view.GetMyRecords(user)).AsQueryable();



                int page = currentPage.HasValue ? currentPage.Value : 1;
                var total = await shippingsQuery.CountAsync();
                var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

                shippings = await shippingsQuery.OrderByDescending(ord => ord.ModifiedOn).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();
                var model = new List<ShippingVm>();
                foreach (var ship in shippings)
                {

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
                    u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
                    u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
                    u.NumFrom = ship.AddressNumFrom;
                    u.NumTo = ship.AddressNumTo;
                    u.SreetFrom = ship.AddressFrom;
                    u.SreetTo = ship.AddressTo;
                    u.TelTarget = ship.TelTarget;
                    u.NameTarget = ship.NameTarget;


                    model.Add(u);

                }

                bool isToday = to.Date == DateTime.Now.AddDays(1).Date;

                ViewBag.BShowAll = showAll;
                ViewBag.ShowAll = showAll.ToString();
                ViewBag.Total = total;
                ViewBag.CurrentPage = page;
                ViewBag.MoreRecord = hasMoreRecord;
                ViewBag.Selected = view.StatusDesc;
                ViewBag.StatusId = view.StatusId;
                ViewBag.FromDay = from.ToString("yyyy-MM-dd");
                ViewBag.ToDay = to.ToString("yyyy-MM-dd");
                ViewBag.IsToday = isToday;
                ViewBag.Title = view.StatusDesc + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");

                return View(model);
            }
        }

        public async Task<ActionResult> Create(string orgid)
        {
            using (var context = new ApplicationDbContext())
            {
                List<Distance> distances = new List<Distance>();
                UserContext userContext = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();

                var city = cache.GetCities(context); // await context.City.ToListAsync();
                var shiptypes = cache.GetShipType(context);

                long increa = 0;
                var model = new ShippingVm();
                var counter = await context.XbzCounter.Take(1).OrderByDescending(o => o.LastNumber).FirstOrDefaultAsync();
                if (counter != null)
                {
                    increa = counter.LastNumber;
                    increa++;
                    counter.LastNumber = increa;
                    context.Entry<XbzCounter>(counter).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                else
                {
                    counter = new XbzCounter();
                    counter.XbzCounterId = Guid.NewGuid();
                    counter.IsActive = true;
                    counter.LastNumber = increa++;
                    context.Entry<XbzCounter>(counter).State = EntityState.Added;
                    await context.SaveChangesAsync();
                }
                model.Number = String.Format("Ran-{0}", increa.ToString().PadLeft(5, '0'));
                model.FastSearch = increa;
                ViewBag.OrderNumber = model.Name;
                var orgs = cache.GetOrgs(context);
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.City = new SelectList(city, "CityId", "Name");
                if (String.IsNullOrEmpty(orgid))
                    orgid = userContext.OrgId.ToString();
                var organid=Guid.Parse(orgid);
                distances = cache.GetDistancesPerOrg(context, organid); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();

                model.OrgId = organid;
                //if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                //{
                //    distances = cache.GetDistancesPerOrg(context, userContext.OrgId); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                //}
                //else
                //{
                //    distances = await context.Distance.ToListAsync();
                //}
                ViewBag.Distance = new SelectList(distances, "DistanceId", "Name");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ShippingVm shippingVm)
        {
            using (var context = new ApplicationDbContext())
            {
                shippingVm.StatusId = Guid.Parse(Helper.Status.Draft);
                var shipping = new Shipping();
                UserContext userContext = new UserContext(AuthenticationManager);

                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                    shipping.Organization_OrgId = userContext.OrgId;
                else
                    shipping.Organization_OrgId = shippingVm.OrgId;

                var userid = userContext.UserId;
                shipping.ShippingId = Guid.NewGuid();
                shipping.FastSearchNumber = shippingVm.FastSearch;
                shipping.Name = shippingVm.Number;

                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;

                var currentDate = DateTime.Now;
                shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
                shipping.CreatedOn = currentDate;
                shipping.CreatedBy = userid;
                shipping.ModifiedOn = currentDate;
                shipping.ModifiedBy = userid;
                shipping.OwnerId = userid;
                shipping.IsActive = true;
                shipping.NotifyType = Notification.Warning; //Notification.Error;//Notification.Warning;
                shipping.NotifyText = Notification.MessageConfirm;
                shipping.CityFrom_CityId = shippingVm.CityForm;
                shipping.AddressFrom = shippingVm.SreetFrom;
                shipping.CityTo_CityId = shippingVm.CityTo;
                shipping.AddressTo = shippingVm.SreetTo;
                shipping.AddressNumTo = shippingVm.NumTo;
                shipping.AddressNumFrom = shippingVm.NumFrom;

                shipping.Recipient = shippingVm.Recipient;
                shipping.TelSource = userContext.Tel;
                shipping.TelTarget = shippingVm.TelTarget;
                shipping.NameSource = userContext.FullName;
                shipping.NameTarget = shippingVm.NameTarget;


                shipping.Distance_DistanceId = shippingVm.DistanceId;
                var shipItem = new ShippingItem()
                    {
                        Name = "זמן המתנה",
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        ShippingItemId = Guid.NewGuid(),
                        IsActive = true,
                        Quantity = 0
                    };
                shipItem.Product_ProductId = Guid.Parse(Helper.ProductType.TimeWait);
                shipping.ShippingItems.Add(shipItem);

                TimeLine tl = new TimeLine
                {
                    Name = "הזמנה חדשה" + "של " + userContext.FullName + " מספר עובד - " + userContext.EmpId + "",
                    Desc = "הזמנה חדשה שנוצרה" + " " + shipping.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm"),
                    CreatedBy = userid,
                    CreatedOn = currentDate,
                    ModifiedBy = userid,
                    ModifiedOn = currentDate,
                    TimeLineId = Guid.NewGuid(),
                    IsActive = true,
                    Status = TimeStatus.New,
                    StatusShipping_StatusShippingId = shippingVm.StatusId
                };
                shipping.TimeLines.Add(tl);
                context.Shipping.Add(shipping);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AddOwnerFollowBy(shipping, userContext, context.Users);

                await context.SaveChangesAsync();
                return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shippingVm.Number, message = "שים לב יש להוסיף פריטי משלוח" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.FindAsync(shipId);

                List<Distance> distances = new List<Distance>();
                var city = cache.GetCities(context);
                var shiptypes = cache.GetShipType(context);
                var orgs = cache.GetOrgs(context);

                var model = new ShippingVm();
                model.Number = shipping.Name;

                model.CityForm = shipping.CityFrom_CityId.GetValueOrDefault();
                model.CityTo = shipping.CityTo_CityId.GetValueOrDefault();
                model.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
                model.ShipTypeId = shipping.ShipType_ShipTypeId.GetValueOrDefault();
                model.FastSearch = shipping.FastSearchNumber;
                model.Id = shipping.ShippingId;

                model.NumFrom = shipping.AddressNumFrom;
                model.NumTo = shipping.AddressNumTo;
                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                model.SreetFrom = shipping.AddressFrom;
                model.SreetTo = shipping.AddressTo;
                model.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                model.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

                model.Recipient = shipping.Recipient;
                //  model.TelSource = shipping.TelSource;
                model.TelTarget = shipping.TelTarget;
                //   model.NameSource = shipping.NameSource;
                model.NameTarget = shipping.NameTarget;

                if (shipping.StatusShipping_StatusShippingId.HasValue)
                {
                    if (shipping.StatusShipping_StatusShippingId.Value == Guid.Parse(Helper.Status.Draft))
                    {
                        shipping.NotifyType = Notification.Warning;
                        shipping.NotifyText = Notification.MessageConfirm;
                    }
                }
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.City = new SelectList(city, "CityId", "Name");
                ViewBag.OrderNumber = shipping.Name;
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");

                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                {
                    Guid orgId = Guid.Empty;
                    distances = cache.GetDistancesPerOrg(context, userContext.OrgId);
                }
                else
                    distances = await context.Distance.ToListAsync();

                ViewBag.Distance = new SelectList(distances, "DistanceId", "Name");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ShippingVm shippingVm)
        {
            using (var context = new ApplicationDbContext())
            {
                var shipping = await context.Shipping.FindAsync(shippingVm.Id);
                UserContext userContext = new UserContext(AuthenticationManager);

                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                    shipping.Organization_OrgId = userContext.OrgId;
                else
                    shipping.Organization_OrgId = shippingVm.OrgId;

                shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
                shipping.Distance_DistanceId = shippingVm.DistanceId;

                shipping.FastSearchNumber = shippingVm.FastSearch;
                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
                shipping.ModifiedOn = DateTime.Now;
                shipping.ModifiedBy = userContext.UserId;
                shipping.IsActive = true;

                shipping.CityFrom_CityId = shippingVm.CityForm;
                shipping.AddressFrom = shippingVm.SreetFrom;
                shipping.CityTo_CityId = shippingVm.CityTo;
                shipping.AddressTo = shippingVm.SreetTo;
                shipping.AddressNumTo = shippingVm.NumTo;
                shipping.AddressNumFrom = shippingVm.NumFrom;

                shipping.Recipient = shippingVm.Recipient;
                //shipping.TelSource = shippingVm.TelSource;
                shipping.TelTarget = shippingVm.TelTarget;
                //    shipping.NameSource = userContext.FullName;
                shipping.NameTarget = shippingVm.NameTarget;

                context.Entry<Shipping>(shipping).State = EntityState.Modified;

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Show(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);

                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(fx => fx.FollowsBy).Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 1)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { UserContext = userContext, Shipping = shipping, Runners = runners });
                ViewBag.OrderNumber = shipping.Name;

                return View(orderModel);
            }
        }

        public async Task<ActionResult> ShipView(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);

                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(fx=>fx.FollowsBy).Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 1)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { UserContext=userContext, Shipping = shipping, Runners = runners });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> Print(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 1)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { Shipping = shipping, Runners = runners });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }
    }
}
