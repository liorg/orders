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
using Michal.Project.Models;
using Michal.Project.Helper;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Michal.Project.Bll;
using Kipodeal.Helper.Cache;


namespace Michal.Project.Controllers
{
    // [Authorize]
    public class DashboardController : Controller
    {
       
        public ActionResult Org(Guid? orgid = null)
        {
            return View();
        }

      
        public ActionResult Supplier(Guid? supplierId=null)
        {
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