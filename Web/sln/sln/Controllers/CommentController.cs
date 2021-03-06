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
using Kipodeal.Helper.Cache;

namespace Michal.Project.Controllers
{
   // [Authorize]
    public class CommentController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> AddComment(string commnetText,string shipIdComment)
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
                comment.Name = user.FullName;
                comment.Shipping_ShippingId = Guid.Parse(shipIdComment);
                context.Entry<Comment>(comment).State = EntityState.Added;
                view.SetJob(comment, user);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("ShipView", "S", new { id = shipIdComment });
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