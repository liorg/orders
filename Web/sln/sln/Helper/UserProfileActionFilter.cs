﻿using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Michal.Project.Helper
{
    public class UserProfileActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var userProfiler = new UserProfiler();
            filterContext.Controller.ViewBag.IsAuthenticated = filterContext.RequestContext.HttpContext.Request.IsAuthenticated;// MembershipService.IsAuthenticated;

            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                userProfiler.IsAdmin = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin);
                userProfiler.AllowConfirm = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleOrgManager) || filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin) || filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAccept);

                userProfiler.AllowAccept = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin);
                userProfiler.AllowRunner = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin) || filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleRunner);

               
            }

            MemeryCacheDataService views = new MemeryCacheDataService();
            filterContext.Controller.ViewBag.Views = views.GetView().ToList();
            filterContext.Controller.ViewBag.UserProfiler=userProfiler;
            filterContext.Controller.ViewBag.Label = " בחר תצוגה אישית ... ";
        } 
    }
    
}