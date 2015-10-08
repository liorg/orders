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
using Michal.Project.Agent;
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

        public async Task<ActionResult> Search(string term)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                orgId = cache.GetOrg(context);

                List<Shipping> shippings = new List<Shipping>();

                var shippingsQuery = context.Shipping.Where(s => s.Organization_OrgId.HasValue &&
                    (s.Organization_OrgId.Value == orgId)).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

                int fastSearch = 0;
                if (int.TryParse(term, out fastSearch))
                {
                    shippingsQuery = shippingsQuery.Where(d => d.FastSearchNumber == fastSearch);
                }
                else
                {
                    shippingsQuery = shippingsQuery.Where(d => d.Name.StartsWith(term));
                }

                int page = 1;//currentPage.HasValue ? currentPage.Value : 1;
                // var total = await shippingsQuery.CountAsync();
                // var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

                shippings = await shippingsQuery.OrderByDescending(ord => ord.ModifiedOn).Skip((page - 1) * Helper.General.MaxRecordsPerSearch).Take(General.MaxRecordsPerSearch).ToListAsync();
                var shippingsItems = new List<ShippingVm>();
                foreach (var ship in shippings)
                {

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";

                    u.TelTarget = ship.TelTarget;
                    u.NameTarget = ship.NameTarget;

                    shippingsItems.Add(u);
                }

                SpecialView specialView = new SpecialView();
                specialView.Items = shippingsItems.AsEnumerable();
                specialView.ClientViewType = ClientViewType.Views;

                specialView.Title = "תוצאות חיפוש עבור " + term;
                // specialView.Total = total;
                specialView.CurrentPage = page;
                // specialView.MoreRecord = hasMoreRecord;

                return View(specialView);
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

                //if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
                //    orgId = Guid.Empty; //user.OrgId;
                orgId = cache.GetOrg(context);
                var showAll = viewAll == null ? user.ShowAll : viewAll.Value;
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1).Date;
                var to = DateTime.Today.AddDays(1).Date;

                if (!String.IsNullOrEmpty(nextDay))
                    to = DateTime.ParseExact(nextDay, "yyyy-MM-dd", null);

                if (!String.IsNullOrEmpty(prevDay))
                    from = DateTime.ParseExact(prevDay, "yyyy-MM-dd", null);

                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && (s.ModifiedOn > from && s.ModifiedOn <= to) && s.Organization_OrgId.HasValue && s.Organization_OrgId.Value == orgId ).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

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
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";

                    u.TelTarget = ship.TelTarget;
                    u.NameTarget = ship.NameTarget;

                    u.TargetAddress = new AddressEditorViewModel();
                    u.TargetAddress.City = ship.Target.CityName;
                    u.TargetAddress.Street = ship.Target.StreetName;
                    u.TargetAddress.Num = ship.Target.StreetNum;
                    u.TargetAddress.ExtraDetail = ship.Target.ExtraDetail;

                    u.SourceAddress = new AddressEditorViewModel();
                    u.SourceAddress.City = ship.Source.CityName;
                    u.SourceAddress.Street = ship.Source.StreetName;
                    u.SourceAddress.Num = ship.Source.StreetNum;
                    u.SourceAddress.ExtraDetail = ship.Source.ExtraDetail;
                    shippingsItems.Add(u);
                }

                bool isToday = to.Date == DateTime.Now.AddDays(1).Date;

                ViewBag.Selected = view.StatusDesc;
                ViewBag.StatusId = view.StatusId;

                SpecialView specialView = new SpecialView();
                specialView.Items = shippingsItems.AsEnumerable();
                specialView.ClientViewType = ClientViewType.Views;

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

        public async Task<ActionResult> Create()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Distance> distances = new List<Distance>();
                UserContext userContext = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();

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

                model.SourceAddress = new AddressEditorViewModel();
                model.SourceAddress.City = userContext.Address.CityName;
                model.SourceAddress.Citycode = userContext.Address.CityCode;
                model.SourceAddress.CitycodeOld = userContext.Address.CityCode;
                model.SourceAddress.Street = userContext.Address.StreetName;
                model.SourceAddress.Streetcode = userContext.Address.StreetCode;
                model.SourceAddress.StreetcodeOld = userContext.Address.StreetCode;
                model.SourceAddress.ExtraDetail = userContext.Address.ExtraDetail;
                model.SourceAddress.Num = userContext.Address.StreetNum;
                model.SourceAddress.UId = userContext.Address.UID;
                model.Direction = 0;//send
                model.TelSource = userContext.Tel;
                model.NameSource = userContext.FullName;

                ViewBag.OrderNumber = model.Name;
                var orgs = cache.GetOrgs(context);
                var sigBacks = cache.GetBackOrder();
                var directions=cache.GetDirection();
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.SigBacks = new SelectList(sigBacks, "Key", "Value");
                ViewBag.Directions = new SelectList(directions, "Key", "Value");
                //if (String.IsNullOrEmpty(orgid))
                //    orgid = userContext.OrgId.ToString();
                //var organid = Guid.Parse(orgid);
                var organid = cache.GetOrg(context);
                distances = cache.GetDistancesPerOrg(context, organid); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();

                model.OrgId = organid;

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
                  MemeryCacheDataService cache = new MemeryCacheDataService();
                  shipping.Organization_OrgId = cache.GetOrg(context);
                //if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                //    shipping.Organization_OrgId = userContext.OrgId;
                //else
                //    shipping.Organization_OrgId = shippingVm.OrgId;

                var userid = userContext.UserId;
                shipping.ShippingId = Guid.NewGuid();
                shipping.FastSearchNumber = shippingVm.FastSearch;
                shipping.Name = shippingVm.Number;
                shipping.SigBackType = shippingVm.SigBackType;
                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
                shipping.Direction = shippingVm.Direction;
                LocationAgent location = new LocationAgent(cache);

                var currentDate = DateTime.Now;
                shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
                shipping.CreatedOn = currentDate;
                shipping.CreatedBy = userid;
                shipping.ModifiedOn = currentDate;
                shipping.ModifiedBy = userid;
                shipping.OwnerId = userid;
                shipping.IsActive = true;
                shipping.NotifyType = (int)AlertStyle.Warning;
                shipping.NotifyText = Notification.MessageConfirm;
                shipping.Direction = shippingVm.Direction;
                shipping.Recipient = shippingVm.Recipient;
                shipping.TelSource = shippingVm.TelSource; // userContext.Tel;
                shipping.TelTarget = shippingVm.TelTarget;
                shipping.NameSource = shippingVm.NameSource;// userContext.FullName;
                shipping.NameTarget = shippingVm.NameTarget;

                shipping.Target.ExtraDetail = shippingVm.TargetAddress.ExtraDetail;

                // location.SetLocation(shippingVm.TargetAddress, shipping.Target);
                await location.SetLocationAsync(shippingVm.TargetAddress, shipping.Target);

                shipping.Source.ExtraDetail = shippingVm.SourceAddress.ExtraDetail;

                // location.SetLocation(shippingVm.SourceAddress, shipping.Source);
                await location.SetLocationAsync(shippingVm.SourceAddress, shipping.Source);

                shipping.Distance_DistanceId = shippingVm.DistanceId;
                //var shipItem = new ShippingItem()
                //    {
                //        Name = "זמן המתנה",
                //        CreatedBy = userid,
                //        CreatedOn = currentDate,
                //        ModifiedBy = userid,
                //        ModifiedOn = currentDate,
                //        ShippingItemId = Guid.NewGuid(),
                //        IsActive = true,
                //        Quantity = 0
                //    };
                //shipItem.Product_ProductId = Guid.Parse(Helper.ProductType.TimeWait);
                //shipping.ShippingItems.Add(shipItem);

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

                var sigBacks = cache.GetBackOrder();
                var shiptypes = cache.GetShipType(context);
                var orgs = cache.GetOrgs(context);

                var model = new ShippingVm();
                model.Number = shipping.Name;
                model.SigBackType = shipping.SigBackType.GetValueOrDefault();
                model.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
                model.ShipTypeId = shipping.ShipType_ShipTypeId.GetValueOrDefault();
                model.FastSearch = shipping.FastSearchNumber;
                model.Id = shipping.ShippingId;

                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

                model.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                model.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

                model.Recipient = shipping.Recipient;
                model.TelTarget = shipping.TelTarget;
                model.NameTarget = shipping.NameTarget;

                model.TelSource = shipping.TelSource;
                model.NameSource = shipping.NameSource;

                model.SourceAddress = new AddressEditorViewModel();
                model.SourceAddress.City = shipping.Source.CityName;
                model.SourceAddress.Citycode = shipping.Source.CityCode;
                model.SourceAddress.CitycodeOld = shipping.Source.CityCode;
                model.SourceAddress.Street = shipping.Source.StreetName;
                model.SourceAddress.Streetcode = shipping.Source.StreetCode;
                model.SourceAddress.StreetcodeOld = shipping.Source.StreetCode;
                model.SourceAddress.ExtraDetail = shipping.Source.ExtraDetail;
                model.SourceAddress.Num = shipping.Source.StreetNum;

                model.TargetAddress = new AddressEditorViewModel();
                model.TargetAddress.City = shipping.Target.CityName;
                model.TargetAddress.Citycode = shipping.Target.CityCode;
                model.TargetAddress.CitycodeOld = shipping.Target.CityCode;
                model.TargetAddress.Street = shipping.Target.StreetName;
                model.TargetAddress.Streetcode = shipping.Target.StreetCode;
                model.TargetAddress.StreetcodeOld = shipping.Target.StreetCode;
                model.TargetAddress.ExtraDetail = shipping.Target.ExtraDetail;
                model.TargetAddress.Num = shipping.Target.StreetNum;

                model.Direction = shipping.Direction;

                if (shipping.StatusShipping_StatusShippingId.HasValue)
                {
                    if (shipping.StatusShipping_StatusShippingId.Value == Guid.Parse(Helper.Status.Draft))
                    {
                        shipping.NotifyType = (int)AlertStyle.Warning;//Notification.Warning;
                        shipping.NotifyText = Notification.MessageConfirm;
                    }
                }
                var directions = cache.GetDirection();
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.OrderNumber = shipping.Name;
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.SigBacks = new SelectList(sigBacks, "Key", "Value");
                ViewBag.Directions = new SelectList(directions, "Key", "Value");
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
                MemeryCacheDataService cache = new MemeryCacheDataService();
                LocationAgent location = new LocationAgent(cache);

                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                    shipping.Organization_OrgId = userContext.OrgId;
                else
                    shipping.Organization_OrgId = shippingVm.OrgId;

                shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
                shipping.Distance_DistanceId = shippingVm.DistanceId;
                shipping.SigBackType = shippingVm.SigBackType;
                shipping.FastSearchNumber = shippingVm.FastSearch;
                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
                shipping.ModifiedOn = DateTime.Now;
                shipping.ModifiedBy = userContext.UserId;
                shipping.IsActive = true;
                shipping.Direction = shippingVm.Direction;
                shipping.Recipient = shippingVm.Recipient;
                shipping.TelTarget = shippingVm.TelTarget;
                shipping.NameTarget = shippingVm.NameTarget;

                shipping.TelSource = shippingVm.TelSource;
                shipping.NameSource = shippingVm.NameSource;

                shipping.Source.ExtraDetail = shippingVm.SourceAddress.ExtraDetail;
                //location.SetLocation(shippingVm.SourceAddress, shipping.Source);
                await location.SetLocationAsync(shippingVm.SourceAddress, shipping.Source);

                shipping.Target.ExtraDetail = shippingVm.TargetAddress.ExtraDetail;
                //location.SetLocation(shippingVm.TargetAddress, shipping.Target);
                await location.SetLocationAsync(shippingVm.TargetAddress, shipping.Target);

                context.Entry<Shipping>(shipping).State = EntityState.Modified;

                await context.SaveChangesAsync();
                return RedirectToAction("Index", "F");
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

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 0)
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
                var shipping = await context.Shipping.Include(fx => fx.FollowsBy).Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 0)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { UserContext = userContext, Shipping = shipping, Runners = runners });
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

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { Shipping = shipping, Runners = runners });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }
    }
}
