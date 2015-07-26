using Microsoft.Owin.Security;
using sln.Dal;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sln.Controllers
{
    public class StatusController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ApprovalRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                if (ship != null)
                {
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = DateTime.Now;
                    ship.ModifiedBy = user.UserId;

                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
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