using sln.Dal;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Helper
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
                userProfiler.AllowSender = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin) || filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleRunner);

            }

            MemeryCacheDataService views = new MemeryCacheDataService();
            filterContext.Controller.ViewBag.Views = views.GetView().ToList();
            filterContext.Controller.ViewBag.UserProfiler=userProfiler;
            filterContext.Controller.ViewBag.Label = " בחר תצוגה אישית ... ";
        }

    }
    //public class LayoutInjecterAttribute : ActionFilterAttribute
    //{
    //    private readonly string _masterName;
    //    public LayoutInjecterAttribute(string masterName)
    //    {
    //        _masterName = masterName;
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        base.OnActionExecuted(filterContext);
    //        var result = filterContext.Result as ViewResult;
    //        if (result != null)
    //        {
    //            result.MasterName = _masterName;
    //            }
    //    }
    //}
}