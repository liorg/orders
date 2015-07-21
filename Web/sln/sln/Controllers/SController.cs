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

        public async Task<ActionResult> Index()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1); Guid orgId = Guid.Empty;
                var shippingsQuery = context.Shipping.Where(s => s.StatusShipping.Name == "3" && s.CreatedOn > from).AsQueryable();
                if (!User.IsInRole("Admin"))
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
                    var created = context.Users.Find(ship.CreatedBy.ToString());

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Name;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.CreatedBy = created != null ? created.FirstName + " " + created.LastName : "";
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
                var city = context.City.ToList();
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
                model.Number = String.Format("Run-{0}", increa); 
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
                    distances =await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                }
                else
                {
                    distances =await context.Distance.ToListAsync();
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
                shippingVm.StatusId = Guid.Parse("00000000-0000-0000-0000-000000000017");
                var shipping = new Shipping();
                ClaimsIdentity id = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                Guid userid = Guid.Empty;
                ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.GroupSid)
                    {
                        shipping.Organization_OrgId = Guid.Parse(claim.Value);

                    }
                    if (claim.Type == ClaimTypes.NameIdentifier)
                    {
                        userid = Guid.Parse(claim.Value);

                    }
                }

                shipping.ShippingId = Guid.NewGuid();
                shipping.Name = "הזמנה בתאריך" + " " + DateTime.Today.ToString("dd/MM/yyyy");

                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
                shipping.CreatedOn = DateTime.Now;
                shipping.CreatedBy = userid;
                shipping.ModifiedOn = DateTime.Now;
                shipping.ModifiedBy = userid;
                shipping.IsActive = true;

                shipping.CityFrom_CityId = shippingVm.CityForm;
                shipping.AddressFrom = shippingVm.SreetFrom;
                shipping.CityFrom_CityId = shippingVm.CityForm;
                shipping.AddressTo = shippingVm.SreetTo;
                shipping.AddressNumTo = shippingVm.NumTo;
                shipping.AddressNumFrom = shippingVm.NumFrom;

                shipping.Distance_DistanceId = shippingVm.DistanceId;

                context.Shipping.Add(shipping);
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
    }
}