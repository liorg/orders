using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Helper
{
    public class UserProfilePictureActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAuthenticated = filterContext.RequestContext.HttpContext.Request.IsAuthenticated;// MembershipService.IsAuthenticated;
            filterContext.Controller.ViewBag.IsAdmin = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin);
           // filterContext.Controller.ViewBag.OrgId = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin);

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