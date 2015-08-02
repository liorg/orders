using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using sln.Dal;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using sln.Bll;

namespace sln.Controllers
{
    [Authorize]
    public class SController : Controller
    {
    
        public async Task<ActionResult> Index(int? viewType, bool? viewAll, int? currentPage)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext user=new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                int order = viewType.HasValue ? viewType.Value : user.DefaultView;
               // if (viewType.HasValue)
                //{
                    var view = cache.GetView().Where(g => g.StatusId == order).FirstOrDefault();
                    if (view != null)
                        ViewBag.Selected = view.StatusDesc;
                    ViewBag.StatusId = view.StatusId;
                    
                //}
                ViewBag.ShowAll = viewAll == null ? user.ShowAll : viewAll.Value;
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1); Guid orgId = Guid.Empty;
                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && s.CreatedOn > from).AsQueryable();
                if (!User.IsInRole(HelperAutorize.RoleAdmin))
                {
                    ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == ClaimTypes.GroupSid)
                        {
                            orgId = Guid.Parse(claim.Value);
                            break;
                        }

                    }
                }
                shippings = await shippingsQuery.Where(sx => sx.Organization_OrgId.HasValue && (sx.Organization_OrgId.Value == orgId || orgId == Guid.Empty)).ToListAsync();
                var model = new List<ShippingVm>();
                foreach (var ship in shippings)
                {
                   // var created = context.Users.Find(ship.CreatedBy.ToString());

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
                    u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
                    u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
                    model.Add(u);

                }
                return View(model);
            }
        }

        public async Task<ActionResult> Create()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Distance> distances = new List<Distance>();

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
                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                {
                    Guid orgId = Guid.Empty;
                    ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == ClaimTypes.GroupSid)
                        {
                            orgId = Guid.Parse(claim.Value);

                            break;
                        }
                    }
                    model.OrgId = orgId;

                    distances = cache.GetDistancesPerOrg(context, orgId); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                }
                else
                {
                    distances = await context.Distance.ToListAsync();
                }
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
                model.Number = shipping.Desc;
                model.NumFrom = shipping.AddressNumFrom;
                model.NumTo = shipping.AddressNumTo;
                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                model.SreetFrom = shipping.AddressFrom;
                model.SreetTo = shipping.AddressTo;
                model.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                model.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
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
                {


                    distances = await context.Distance.ToListAsync();
                }
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

                shipping.Distance_DistanceId = shippingVm.DistanceId;
                context.Entry<Shipping>(shipping).State = EntityState.Modified;

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Show(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 1)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { Shipping = shipping, Runners = runners });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> ShipView(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

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
