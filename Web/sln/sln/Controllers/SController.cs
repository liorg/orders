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

namespace sln.Controllers
{
    [Authorize]
    public class SController : Controller
    {

        public ActionResult Index()
        {
            using (var Db = new ApplicationDbContext())
            {
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Now.AddDays(-1); Guid orgId=Guid.Empty;
                var shippingsQuery = Db.Shipping.Where(s => s.StatusShipping.Name == "3" && s.CreatedOn > from).AsQueryable();
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
                        //if (claim.Type == ClaimTypes.NameIdentifier)
                        //{
                        //    var dd = claim.Value;

                        //}
                    }
                    shippings = shippingsQuery.Where(sx => sx.Organization_OrgId.HasValue && sx.Organization_OrgId.Value == orgId).ToList();
                }
                var model = new List<ShippingVm>();
                foreach (var ship in shippings)
                {
                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Name;
                    u.Name = ship.Name;
                    model.Add(u);
                }
                return View(model);
            }
        }

        public ActionResult Create()
        {
            using (var Db = new ApplicationDbContext())
            {
                var StatusShipping = Db.StatusShipping.ToList();
                var model = new ShippingVm();
                ViewBag.StatusShipping = new SelectList(StatusShipping, "StatusShippingId", "Name");

                return View(model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(ShippingVm shippingVm)
        {
            using (var context = new ApplicationDbContext())
            {
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
                shipping.Name = shippingVm.Name;
                shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
                shipping.CreatedOn = DateTime.Now;
                shipping.CreatedBy = userid;
                shipping.ModifiedOn   = DateTime.Now;
                shipping.ModifiedBy = userid;
                shipping.IsActive = true;
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