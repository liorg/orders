using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using sln.Models;
using sln.Helper;
using sln.Dal;
using sln.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;
using sln.Bll;
using Kipodeal.Helper.Cache;

namespace sln.Controllers
{
   // [Authorize]
    public class OrgController : Controller
    {
       
        public async Task<ActionResult> CreateShipByOrg()
        {
             using (var context = new ApplicationDbContext())
            {
                ViewLogic view = new ViewLogic();
              //  UserContext user = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                ViewBag.Orgs = cache.GetOrgs(context).ToList();
               

            }
            return View();
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