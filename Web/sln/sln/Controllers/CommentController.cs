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
        public async Task<ActionResult> AddComment(string commnetText,string shipId)
        {
            Comment comment = new Comment();
            using (var context = new ApplicationDbContext())
            {
                ViewLogic view = new ViewLogic();
                UserContext user = new UserContext(AuthenticationManager);
                comment.CommentId = Guid.NewGuid();
                comment.CreatedOn = DateTime.Now;
                comment.CreatedBy = user.UserId;
                comment.ModifiedOn = DateTime.Now;
                comment.ModifiedBy = user.UserId;
                comment.IsActive = true;
                comment.Desc = commnetText;
                view.SetJob(comment, User);

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