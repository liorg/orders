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

namespace sln.Controllers
{
    [Authorize]
    public class SController : Controller
    {
        public async Task<ActionResult> Index(int? viewType,int? currentPage)
        {
            using (var context = new ApplicationDbContext())
            {
                MemeryCacheDataService cache = new MemeryCacheDataService(context);
              
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1); Guid orgId = Guid.Empty;
                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.Name == "3" && s.CreatedOn > from).AsQueryable();
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
               // ViewOrder<IEnumerable<ShippingVm>> viewOrder = new ViewOrder<IEnumerable<ShippingVm>>(); 
                foreach (var ship in shippings)
                {
                    var created = context.Users.Find(ship.CreatedBy.ToString());

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.CreatedBy = created != null ? created.FirstName + " " + created.LastName : "";
                    u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
                    u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
                    model.Add(u);

                }
                //viewOrder.Model = model;
                //viewOrder.ViewTypes = new List<Models.ViewType>();

                return View(model);
            }
        }

        public async Task<ActionResult> Create()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Distance> distances = new List<Distance>();
                var city = await context.City.ToListAsync();
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

                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");

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
                    distances = await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
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
                shippingVm.StatusId = Guid.Parse(Helper.Status.New);
                var shipping = new Shipping();
                UserContext userContext = new UserContext(AuthenticationManager);
                //var orgid = Guid.Empty;
                //ClaimsIdentity id = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                //Guid userid = Guid.Empty;
                //ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                //foreach (var claim in claimsIdentity.Claims)
                //{
                //    if (claim.Type == ClaimTypes.GroupSid)
                //        orgid = Guid.Parse(claim.Value);

                //    if (claim.Type == ClaimTypes.NameIdentifier)
                //        userid = Guid.Parse(claim.Value);
                //    ;
                //}
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
                shipping.CreatedOn = currentDate;
                shipping.CreatedBy = userid;
                shipping.ModifiedOn = currentDate;
                shipping.ModifiedBy = userid;
                shipping.OwnerId = userid;
                shipping.IsActive = true;

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
                    Name = "הזמנה חדשה",
                    Desc = "הזמנה חדשה שנוצרה" + shipping.Name +"בתאריך "+currentDate.ToLongTimeString(),
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
                return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shippingVm.Number ,message="שים לב יש להוסיף פריטי משלוח"});
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.FindAsync(shipId);
                List<Distance> distances = new List<Distance>();
                var city = await context.City.ToListAsync();
                var model = new ShippingVm();
                model.Number = shipping.Name;
                ViewBag.OrderNumber = shipping.Name;
                model.CityForm = shipping.CityFrom_CityId.GetValueOrDefault();
                model.CityTo = shipping.CityTo_CityId.GetValueOrDefault();
                model.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
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

                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");
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
                    //model.OrgId = orgId;
                    distances = await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
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
                ClaimsIdentity id = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                //Guid userid = Guid.Empty; var orgId = Guid.Empty;
                UserContext userContext = new UserContext(AuthenticationManager);

                //ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                //foreach (var claim in claimsIdentity.Claims)
                //{
                //    if (claim.Type == ClaimTypes.GroupSid)
                //      orgId = Guid.Parse(claim.Value);

                //    if (claim.Type == ClaimTypes.NameIdentifier)
                //      userid = Guid.Parse(claim.Value);

                //}
                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                    shipping.Organization_OrgId = userContext.OrgId;
                else
                    shipping.Organization_OrgId = shippingVm.OrgId;

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
                //context. (shipping);

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
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(ic=>ic.ShippingItems).Include(tl=>tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
               
                if(shipping.ShippingItems==null || shipping.ShippingItems.Count<=1)
                      return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(),order=shipping.Name,message="יש לבחור פריטים  למשלוח" });
                
                List<Distance> distances = new List<Distance>();
                var city = await context.City.ToListAsync();
                var orderModel = new OrderView();

                orderModel.Status = new StatusVm();
                orderModel.Status.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                orderModel.Status.Name = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                orderModel.Status.MessageType = Notification.Warning; //Notification.Error;//Notification.Warning;
                orderModel.Status.Message = Notification.MessageConfirm;

                orderModel.ShippingVm = new ShippingVm();
                orderModel.ShippingVm.Number = shipping.Name;
                ViewBag.OrderNumber = shipping.Name;
                orderModel.ShippingVm.CityForm = shipping.CityFrom_CityId.GetValueOrDefault();
                orderModel.ShippingVm.CityTo = shipping.CityTo_CityId.GetValueOrDefault();
                orderModel.ShippingVm.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
                orderModel.ShippingVm.FastSearch = shipping.FastSearchNumber;
                orderModel.ShippingVm.Id = shipping.ShippingId;
                orderModel.ShippingVm.Number = shipping.Desc;
                orderModel.ShippingVm.NumFrom = shipping.AddressNumFrom;
                orderModel.ShippingVm.NumTo = shipping.AddressNumTo;
                orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                orderModel.ShippingVm.SreetFrom = shipping.AddressFrom;
                orderModel.ShippingVm.SreetTo = shipping.AddressTo;
                orderModel.ShippingVm.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                orderModel.ShippingVm.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                orderModel.ShippingVm.CreatedOn = shipping.CreatedOn.Value.ToString("dd/MM/yyyy");
                orderModel.ShippingVm.ModifiedOn = shipping.ModifiedOn.Value.ToString("dd/MM/yyyy");
                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");
                ViewBag.City = new SelectList(city, "CityId", "Name");

                orderModel.ShippingVm.CityFormName=shipping.CityFrom!=null ?shipping.CityFrom.Name:"";
                orderModel.ShippingVm.CityToName=shipping.CityTo!=null ?shipping.CityTo.Name:"";

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
                    //model.OrgId = orgId;
                    distances = await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                }
                else
                {
                    distances = await context.Distance.ToListAsync();
                }
                ViewBag.Distance = new SelectList(distances, "DistanceId", "Name");
                var timeLineVms = new List<TimeLineVm>();
                foreach (var timeline in shipping.TimeLines)
                {
                    timeLineVms.Add(new TimeLineVm {Title=timeline.Name,CreatedOn=timeline.CreatedOn.GetValueOrDefault(),  TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status });
                }

                orderModel.TimeLineVms=timeLineVms;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> ShipView(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 1)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                List<Distance> distances = new List<Distance>();
                var city = await context.City.ToListAsync();
                var orderModel = new OrderView();

                orderModel.Status = new StatusVm();
                orderModel.Status.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                orderModel.Status.Name = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                orderModel.Status.MessageType = Notification.Warning; //Notification.Error;//Notification.Warning;
                orderModel.Status.Message = Notification.MessageConfirm;

                orderModel.ShippingVm = new ShippingVm();
                orderModel.ShippingVm.Number = shipping.Name;
                ViewBag.OrderNumber = shipping.Name;
                orderModel.ShippingVm.CityForm = shipping.CityFrom_CityId.GetValueOrDefault();
                orderModel.ShippingVm.CityTo = shipping.CityTo_CityId.GetValueOrDefault();
                orderModel.ShippingVm.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
                orderModel.ShippingVm.FastSearch = shipping.FastSearchNumber;
                orderModel.ShippingVm.Id = shipping.ShippingId;
                orderModel.ShippingVm.Number = shipping.Desc;
                orderModel.ShippingVm.NumFrom = shipping.AddressNumFrom;
                orderModel.ShippingVm.NumTo = shipping.AddressNumTo;
                orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                orderModel.ShippingVm.SreetFrom = shipping.AddressFrom;
                orderModel.ShippingVm.SreetTo = shipping.AddressTo;
                orderModel.ShippingVm.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
                orderModel.ShippingVm.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
                orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
                orderModel.ShippingVm.CreatedOn = shipping.CreatedOn.Value.ToString("dd/MM/yyyy");
                orderModel.ShippingVm.ModifiedOn = shipping.ModifiedOn.Value.ToString("dd/MM/yyyy");
                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");
                ViewBag.City = new SelectList(city, "CityId", "Name");

                orderModel.ShippingVm.CityFormName = shipping.CityFrom != null ? shipping.CityFrom.Name : "";
                orderModel.ShippingVm.CityToName = shipping.CityTo != null ? shipping.CityTo.Name : "";

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
                    //model.OrgId = orgId;
                    distances = await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                }
                else
                {
                    distances = await context.Distance.ToListAsync();
                }
                ViewBag.Distance = new SelectList(distances, "DistanceId", "Name");
                var timeLineVms = new List<TimeLineVm>();
                foreach (var timeline in shipping.TimeLines.OrderBy(t=>t.CreatedOn))
                {
                    timeLineVms.Add(new TimeLineVm { Title=timeline.Name, TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status,CreatedOn=timeline.CreatedOn.GetValueOrDefault() });
                }

                orderModel.TimeLineVms = timeLineVms;
                return View(orderModel);
            }
        }

    }
}