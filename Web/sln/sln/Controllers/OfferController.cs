﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Michal.Project.Models;
using Michal.Project.Helper;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Michal.Project.Bll;
using Michal.Project.Models.View;


namespace Michal.Project.Controllers
{
    public class OfferController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Index(int offerType, int state)//state=>1=New,2=InProccess, 3=End,4=Cancel
        {
            using (var context = new ApplicationDbContext())
            {
                List<OfferVm> offers = new List<OfferVm>();
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();

                orgId = cache.GetOrg(context);
                return View(offers);
            }
        }


        public async Task<ActionResult> CreateOrder()
        {
            using (var context = new ApplicationDbContext())
            {
                OrderDetail order = new OrderDetail();
                var user = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();

                Guid orgId = cache.GetOrg(context);
                return View(order);
            }
        }

        public async Task<ActionResult> CreateOffer()
        {
            using (var context = new ApplicationDbContext())
            {
                OfferDetail offer = new OfferDetail();
                var user = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();

                Guid orgId = cache.GetOrg(context);
                return View(offer);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(OfferItemVm offerItemVm)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();

                orgId = cache.GetOrg(context);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "F");
            }
          
        }
    }
}