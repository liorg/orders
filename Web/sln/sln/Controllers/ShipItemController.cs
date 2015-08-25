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

namespace Michal.Project.Controllers
{
    [Authorize]
    public class ShipItemController : Controller
    {

        public async Task<ActionResult> RemoveItem(string id,string order)
        {
            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                Guid userid = user.UserId;
                Guid shipItemId = Guid.Parse(id); Guid shipId = Guid.Empty;
                var shipItem = await context.ShippingItem.FindAsync(shipItemId);
                if (shipItem != null)
                {
                   shipId = shipItem.Shipping_ShippingId.Value;
               
                }
                
                context.Entry<ShippingItem>(shipItem).State = EntityState.Deleted;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = shipId.ToString(), order = order });
            }
        }

        public async Task<ActionResult> Index(string id, string order, string message)
        {
            using (var context = new ApplicationDbContext())
            {
                ViewBag.ShipId = id;
                ViewBag.OrderNumber = order;
                Guid shipId = Guid.Parse(id);
                var shippingItems = await context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product != null && s.Product.IsCalculatingShippingInclusive == false).ToListAsync();
                ViewBag.Message = String.IsNullOrEmpty( message)?"":message;
                var model = new List<ShippingItemVm>();
                foreach (var shipItem in shippingItems)
                {

                    var u = new ShippingItemVm();
                    u.Id = shipItem.ShippingItemId;
                    u.ProductName = shipItem.Product != null ? shipItem.Product.Name : "";
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
                model.Total = 1;

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
                UserContext user = new UserContext(AuthenticationManager);
                userid = user.UserId;

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
                return RedirectToAction("Index", new { id = shippingItemVm.ShipId.ToString(),order=shippingItemVm.OrderNumber });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var model = new ShippingItemVm();
                model.ShipId = Guid.Parse(id);

                var shipItem = await context.ShippingItem.FindAsync(model.ShipId);
                if (shipItem == null || shipItem.Shipping == null)
                    throw new ArgumentNullException("shipItem");
                var ship = shipItem.Shipping;
                var org = ship.Organization_OrgId;
                var products = await context.Product.Where(s => s.Organizations.Any(e => org.HasValue && e.OrgId == org.Value)).ToListAsync();
                model.OrderNumber = ship.Name;
                model.Id = shipItem.ShippingItemId;
                model.ProductId = shipItem.Product_ProductId.GetValueOrDefault();
                model.Total = Convert.ToInt32(shipItem.Quantity);

                ViewBag.Products = new SelectList(products, "ProductId", "Name");
                ViewBag.ShipId = ship.ShippingId;
                ViewBag.OrderNumber = model.OrderNumber;

                return View(model);

            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ShippingItemVm shippingItemVm)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                var shippingItem = await context.ShippingItem.FindAsync(shippingItemVm.Id);

                ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userid = Guid.Parse(claim.Value);
                }

                shippingItem.Quantity = shippingItemVm.Total;

                shippingItem.ModifiedOn = DateTime.Now;
                shippingItem.ModifiedBy = userid;

                context.Entry<ShippingItem>(shippingItem).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = shippingItem.Shipping_ShippingId.Value.ToString(), order = shippingItemVm.OrderNumber });
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