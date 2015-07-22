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
    public class ShipItemController : Controller
    {

        public async Task<ActionResult> Index(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                ViewBag.ShipId = id;
                Guid shipId = Guid.Parse(id);
                var shippingItems = await context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product!=null && s.Product.IsCalculatingShippingInclusive==false).ToListAsync();
               
                var model = new List<ShippingItemVm>();
                foreach (var shipItem in shippingItems)
                {

                    var u = new ShippingItemVm();
                    u.Id = shipItem.ShippingItemId;
                    u.ProductName = shipItem.Product != null ? shipItem.Product.Name : "";
                    //u.Total=shipItem.
                    u.Name = shipItem.Name;
                    u.Total = Convert.ToInt32(shipItem.Quantity);
                    model.Add(u);
                }
                return View(model);
            }
        }

        public async Task<ActionResult> Create(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var model = new ShippingItemVm();
                model.ShipId = Guid.Parse(id);

                var ship = await context.Shipping.FindAsync(model.ShipId);
                var products = await context.Product.Where(s => s.Organizations.Any(e => ship.Organization_OrgId.HasValue && e.OrgId == ship.Organization_OrgId.Value)).ToListAsync();
                model.OrderNumber = ship.Name;
                ViewBag.Products = new SelectList(products, "ProductId", "Name");
                ViewBag.ShipId = id;
                ViewBag.OrderNumber = ship.Name;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ShippingItemVm shippingItemVm)
        {
            using (var context = new ApplicationDbContext())
            {
                var shippingItem = new ShippingItem();
                 Guid userid = Guid.Empty;

                ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                 foreach (var claim in claimsIdentity.Claims)
                 {
                     if (claim.Type == ClaimTypes.NameIdentifier)
                         userid = Guid.Parse(claim.Value);

                 }

                shippingItem.ShippingItemId = Guid.NewGuid();
                shippingItem.Shipping_ShippingId = shippingItemVm.ShipId;
                shippingItem.Quantity = shippingItemVm.Total;
                shippingItem.Product_ProductId = shippingItemVm.ProductId;
                shippingItem.CreatedOn = DateTime.Now;
                shippingItem.CreatedBy = userid;
                shippingItem.ModifiedOn = DateTime.Now;
                shippingItem.ModifiedBy = userid;
                shippingItem.IsActive = true;
                context.ShippingItem.Add(shippingItem);

                await context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = shippingItemVm.ShipId.ToString() });
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
        public async Task<ActionResult> Edit(ShippingVm shippingVm)
        {
            using (var context = new ApplicationDbContext())
            {
                var shipping = await context.Shipping.FindAsync(shippingVm.Id);
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

                shipping.FastSearchNumber = shippingVm.FastSearch;
                shipping.Name = shippingVm.Number;
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

    }
}