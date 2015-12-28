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
using Michal.Project.Contract.DAL;
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
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : General.Empty;
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : General.Empty;
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy HH:mm") : General.Empty;

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

                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && (s.ModifiedOn > from && s.ModifiedOn <= to) && s.Organization_OrgId.HasValue && s.Organization_OrgId.Value == orgId).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

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
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : General.Empty;
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : General.Empty;
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy HH:mm") : General.Empty;

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
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                var model = await logic.OnPreCreateShip(userContext);

                var shiptypes = generalRepo.GetShipType();

                ViewBag.OrderNumber = model.Name;

                var orgs = generalRepo.GetOrgs();
                var sigBacks = generalRepo.GetBackOrder();
                var directions = generalRepo.GetDirection();

                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.SigBacks = new SelectList(sigBacks, "Key", "Value");
                ViewBag.Directions = new SelectList(directions, "Key", "Value");

                var org = generalRepo.GetOrgEntity();
                var organid = org.OrgId;
                generalRepo.GetDistancesPerOrg(organid);
                distances = generalRepo.GetDistancesPerOrg(organid);
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
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                UserContext userContext = new UserContext(AuthenticationManager);
                var id = await logic.OnPostCreateShip(shippingVm, userContext);

                await context.SaveChangesAsync();
                return RedirectToAction("Index", "ShipItem", new { Id = id.ToString(), order = shippingVm.Number, message = "שים לב יש להוסיף פריטי משלוח" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);

                List<Distance> distances = new List<Distance>();

                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                var model = await logic.OnPreUpdateShip(shipId, userContext);

                var sigBacks = generalRepo.GetBackOrder();
                var shiptypes = generalRepo.GetShipType();
                var orgs = generalRepo.GetOrgs();
                var org = generalRepo.GetOrgEntity();
                var organid = org.OrgId;

                var directions = generalRepo.GetDirection();
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.OrderNumber = model.Name;
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.SigBacks = new SelectList(sigBacks, "Key", "Value");
                ViewBag.Directions = new SelectList(directions, "Key", "Value");
                Guid orgId = Guid.Empty;
                distances = generalRepo.GetDistancesPerOrg(organid);

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

                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());

                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                await logic.OnPostUpdateShip(shippingVm, userContext);

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
                //var shipping = await context.Shipping.Include(fx => fx.FollowsBy).Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(com => com.Comments).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 0)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                // var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { UserContext = userContext, Shipping = shipping });
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
                // var shipping = await context.Shipping.Include(fx => fx.FollowsBy).Include(ic => ic.ShippingItems).Include(att => att.AttachmentsShipping).Include(com => com.Comments).Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
                var shipping = await context.Shipping.Include(ic => ic.ShippingItems).Include(com => com.Comments).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                if (shipping.ShippingItems == null || shipping.ShippingItems.Count <= 0)
                    return RedirectToAction("Index", "ShipItem", new { Id = shipping.ShippingId.ToString(), order = shipping.Name, message = "יש לבחור פריטים  למשלוח" });

                ViewLogic view = new ViewLogic();
                // var runners = cacheProvider.GetRunners(context);
                var orderModel = view.GetOrder(new OrderRequest { UserContext = userContext, Shipping = shipping });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> Tl(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);

                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
             //   var shipping = await context.Shipping.Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
               
                ViewLogic view = new ViewLogic(shippingRepository, userRepository, generalRepo);
                var orderModel =await  view.GetTimeLine(shipId);
                ViewBag.OrderNumber = orderModel.Name;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> User(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                Guid shipId = Guid.Parse(id);
                
                ViewLogic view = new ViewLogic(shippingRepository, userRepository, generalRepo);
                var orderModel = await view.GetUser(shipId);
                ViewBag.OrderNumber = orderModel.Name;
                ViewBag.Runners = new SelectList(orderModel.Runners, "Id", "FullName");
                return View(orderModel);
            }
        }

        public async Task<ActionResult> Print(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                MemeryCacheDataService cacheProvider = new MemeryCacheDataService();
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                ViewLogic view = new ViewLogic();
                var orderModel = view.GetOrder(new OrderRequest { Shipping = shipping });
                ViewBag.OrderNumber = shipping.Name;
                return View(orderModel);
            }
        }

        public async Task<ActionResult> ShippingByRunnerId(Guid? userid)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext userContext = new UserContext(AuthenticationManager);
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);

                if (!userid.HasValue)
                    userid = userContext.UserId;
                //var shipping = await context.Shipping.FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

                ViewLogic view = new ViewLogic(shippingRepository, userRepository, generalRepo);
                var result = await view.GetShippingByUser(userid.Value);
                return View(result);
            }
        }

    }
}
