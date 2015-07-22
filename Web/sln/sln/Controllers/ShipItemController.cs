﻿using Microsoft.AspNet.Identity;
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

        public async Task<ActionResult> Remove(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                ViewBag.ShipId = id;
                Guid userid = Guid.Empty;

                ClaimsIdentity claimsIdentity = AuthenticationManager.User.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userid = Guid.Parse(claim.Value);

                }
                Guid shipId = Guid.Parse(id);
                var shipItem = await context.ShippingItem.FindAsync(shipId);
                if (shipItem != null)
                {
                    shipItem.IsActive = false;
                    shipItem.ModifiedOn = DateTime.Now;
                    shipItem.ModifiedBy = userid;
                }

                context.Entry<ShippingItem>(shipItem).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return View("Index", new { id = shipItem.Shipping_ShippingId.ToString() });
            }
        }

        public async Task<ActionResult> Index(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                ViewBag.ShipId = id;
                Guid shipId = Guid.Parse(id);
                var shippingItems = await context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product != null && s.Product.IsCalculatingShippingInclusive == false).ToListAsync();

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
                return RedirectToAction("Index", new { id = shippingItem.Shipping_ShippingId.Value.ToString() });
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