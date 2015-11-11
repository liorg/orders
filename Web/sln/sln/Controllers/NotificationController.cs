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
        public async Task<ActionResult> Index()
        {
            var user = new UserContext(AuthenticationManager);
            using (var context = new ApplicationDbContext())
            {
                ViewBag.UserId = user.UserId;
                List<NotifyItem> items = context.NotifyMessage.Where(u => u.UserId == user.UserId && u.IsActive == true).Select(
                     m => new NotifyItem
                     {
                         Id = m.NotifyMessageId,
                         Title=m.Title,
                         IsRead=m.IsRead,
                         Body = m.Body,
                         Url = m.ToUrl

                     }).ToList();

                return View(items);
            }
        }
    }
}