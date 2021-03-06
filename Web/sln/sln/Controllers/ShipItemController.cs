﻿using Microsoft.AspNet.Identity;
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
using Michal.Project.Contract.DAL;
using Michal.Project.Agent;
using Michal.Project.Bll;

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
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                var vm = new ShippingItemsVm();
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);


                ViewBag.ShipId = id;


                ViewBag.OrderNumber = order;
                Guid shipId = Guid.Parse(id);
                vm.Id = shipId;
                vm.Name = order;
                vm.ShippingItems = await logic.GetItemsShip(shipId);   //await context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product != null && s.Product.IsCalculatingShippingInclusive == false).ToListAsync();
                ViewBag.Message = String.IsNullOrEmpty(message) ? "" : message;

                return View(vm);
            }
        }

        public async Task<ActionResult> Create(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var cache = new MemeryCacheDataService();
               var orgid=cache.GetOrg(context);
                var model = new ShippingItemVm();
                model.ShipId = Guid.Parse(id);
                model.Total = 1;

                var ship = await context.Shipping.FindAsync(model.ShipId);
                var products = cache.GetProducts(context, orgid); //await context.Product.Where(s => s.Organizations.Any(e => ship.Organization_OrgId.HasValue && e.OrgId == ship.Organization_OrgId.Value)).ToListAsync();
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
                var cache = new MemeryCacheDataService();
                var model = new ShippingItemVm();
                model.ShipId = Guid.Parse(id);

                var shipItem = await context.ShippingItem.FindAsync(model.ShipId);
                if (shipItem == null || shipItem.Shipping == null)
                    throw new ArgumentNullException("shipItem");
                var ship = shipItem.Shipping;
                var org = ship.Organization_OrgId;
                var orgid = cache.GetOrg(context);
                var products = cache.GetProducts(context, orgid);//await context.Product.Where(s => s.Organizations.Any(e => org.HasValue && e.OrgId == org.Value)).ToListAsync();
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
                var shippingItem = await context.ShippingItem.FindAsync(shippingItemVm.Id);
                UserContext userContext = new UserContext(AuthenticationManager);
                
                var userid = userContext.UserId;
                shippingItem.Quantity = shippingItemVm.Total;
                shippingItem.Product_ProductId = shippingItemVm.ProductId;
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