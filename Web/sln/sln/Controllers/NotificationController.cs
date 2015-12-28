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
using Michal.Project.Fasade;

namespace Michal.Project.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public async Task<ActionResult> Index(int? currentPage)
        {
            var user = new UserContext(AuthenticationManager);
            using (var context = new ApplicationDbContext())
            {
                var notifyRepo = new NotificationRepository(context);
                var logic = new NotifyLogic(notifyRepo);
                var model = await logic.GetNotifiesUserAsync(user, currentPage);
                ViewBag.UserId = user.UserId;
                return View(model);
            }
        }
        public async Task<ActionResult> Remove(Guid id)
        {
            var user = new UserContext(AuthenticationManager);
            using (var context = new ApplicationDbContext())
            {
                var notifyRepo = new NotificationRepository(context);
                var logic = new NotifyLogic(notifyRepo);
                await logic.Remove(id);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

    }
}