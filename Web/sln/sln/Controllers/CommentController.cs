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
    public class CommentController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> AddComment(string commnet,string shipId)
        {
            Comment comment = new Comment();
            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                comment.CommentId = Guid.NewGuid();
                comment.CreatedOn = DateTime.Now;
                comment.CreatedBy = user.UserId;
                comment.IsActive = true;
               // if(User.IsInRole
               

            }
            return RedirectToAction("ShipView", "S", new { id = shipId });
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